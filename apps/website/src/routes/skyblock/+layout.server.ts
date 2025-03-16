import { mongoBot } from '$lib/server/mongoBot';
import type { LayoutServerLoad } from './$types';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';

export const load: LayoutServerLoad = async (event) => {
	const token = event.cookies.get('session') ?? null;

	if (token === null) {
		return utilityServer.redirectToLogin(event.route);
	}

	const session = await sessionServer.validateSessionToken(token);

	if (session === null) {
		sessionServer.deleteDiscordId(event);
		sessionServer.deleteSessionTokenCookie(event);
		return utilityServer.redirectToLogin(event.route);
	}

	const foundUser = await mongoBot.MONGODB_C_USERS.FindOne({ Username: session.Username });

	if (foundUser === null) {
		// TODO: log that forgery was successfully made, as the user couldnt be found, but the session was validated.
		return utilityServer.redirectToLogin(event.route);
	}

	sessionServer.setDiscordId(event, foundUser.DiscordId, session.ExpiresAt);
	sessionServer.setSessionTokenCookie(event, token, session.ExpiresAt);

	return {
		title: 'Skyblock',
		description: 'Edit your Hypixel Skyblock details! Currently contains features for the Auction and Bazaar.'
	};
};
