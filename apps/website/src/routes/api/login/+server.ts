import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/common/apiContracts';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';
import { json, type RequestHandler } from '@sveltejs/kit';
import { discordServer } from '$lib/server/discordServer';
import { LogLevel } from '$lib/enums';

export const POST: RequestHandler = async (event) => {
	const { params } = API_CONTRACTS['POST=>/api/login'];
	const { user } = (await event.request.json()) as typeof params;
	const { Password, Provider, Username } = user;

	const token = event.cookies.get('session') ?? null;
	let deletedToken = false;

	if (token !== null) {
		const session = await sessionServer.validateSessionToken(token);

		if (session === null) {
			sessionServer.deleteSessionTokenCookie(event);
		} else {
			await mongoBot.MONGODB_C_SESSIONS.DeleteOne({ id: session.ID });
			deletedToken = true;
		}
	}

	let foundUser = null;

	// TODO: implement email
	switch (Provider) {
		case 'discord':
			const _user = await mongoBot.MONGODB_C_AUTH_DISCORD.FindOne({ Username });

			if (!_user) {
				utilityServer.logServer(LogLevel.WARN, 'Could not find discord username when logging in.');
				return utilityServer.errorInvalidCredentials();
			}

			foundUser = await mongoBot.MONGODB_C_AUTH_USERS.FindOne({ _id: { $eq: _user._id_AuthUser } });

			// verify that the access the user provided to login is valid
			const result = await discordServer.getMe(Password);

			if (result.id !== _user.UserId) {
				utilityServer.logServer(LogLevel.WARN, 'Access token given is invalid/expired!');
				return utilityServer.errorInvalidCredentials();
			}

			break;
		case 'email':
			break;
		default:
			return utilityServer.errorInvalidCredentials();
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
	const session = await sessionServer.createSession(newSession, foundUser._id.toHexString());
	sessionServer.setSessionTokenCookie(event, newSession, session.ExpiresAt);

	return json('');
};
