import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';
import { error, json, type RequestHandler } from '@sveltejs/kit';

export const POST: RequestHandler = async (event) => {
	const { params } = API_CONTRACTS['POST=>/api/login'];
	const { user } = (await event.request.json()) as typeof params;
	const foundUser = await mongoBot.MONGODB_C_USERS.FindOne({ Username: user.Username, Password: user.Password });
	const token = event.cookies.get('session') ?? null;
	let deletedToken = false;

	if (token !== null) {
		const session = await sessionServer.validateSessionToken(token);

		if (session === null) {
			sessionServer.deleteDiscordId(event);
			sessionServer.deleteSessionTokenCookie(event);
		} else {
			await mongoBot.MONGODB_C_SESSIONS.DeleteOne({ id: session.ID });
			deletedToken = true;
		}
	}

	if (foundUser === null) {
		if (deletedToken) {
			// TODO: log that the user had a valid session token, but invalid credentials here. this can only happen if:
			// - the client had an open session at the time of credentials change
			// - the client successfully grabbed the correct session token (forgery/leak), and attempted a POST request here
		}

		return utilityServer.errorInvalidCredentials();
	}

	const newSession = sessionServer.generateSessionToken();
	const session = await sessionServer.createSession(newSession, foundUser.DiscordId, foundUser.Username);
	sessionServer.setSessionTokenCookie(event, newSession, session.ExpiresAt);
	sessionServer.setDiscordId(event, foundUser.DiscordId, session.ExpiresAt);

	const cookies = event.cookies.getAll();

	for (const cookie of cookies) {
		console.log(`${cookie.name}: ${cookie.value}`);
	}

	return json('');
};
