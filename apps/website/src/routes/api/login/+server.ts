import { API_CONTRACTS } from '$lib/other/apiContracts';
import { mongoBot } from '$lib/server/mongoBot';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';
import { error, json, type RequestHandler } from '@sveltejs/kit';

export const POST: RequestHandler = async (event) => {
	const { params } = API_CONTRACTS['POST=>/api/login'];
	const { user } = (await event.request.json()) as typeof params;
	const foundUser = await mongoBot.MONGODB_C_USERS.FindOne(user);
	const token = event.cookies.get('session') ?? null;
	let deletedToken = false;

	if (token !== null) {
		const session = await sessionServer.validateSessionToken(token);

		if (session === null) {
			sessionServer.deleteDiscordId(event);
			sessionServer.deleteSessionTokenCookie(event);
		} else {
			await mongoBot.MONGODB_C_SESSIONS.DeleteOne({ id: session.id });
			deletedToken = true;
		}
	}

	if (!foundUser) {
		if (deletedToken) {
			// TODO: log that the user had a valid session token, but invalid credentials here. this can only happen if:
			// - the client had an open session at the time of credentials change
			// - the client successfully grabbed the correct session token (forgery/leak), and attempted a POST request here
		}

		return utilityServer.errorInvalidCredentials();
	}

	const newSession = sessionServer.generateSessionToken();
	const session = await sessionServer.createSession(newSession, user.username);
	sessionServer.setSessionTokenCookie(event, newSession, session.expiresAt);
	sessionServer.setDiscordId(event, foundUser.discordId, session.expiresAt);

	return json('');
};
