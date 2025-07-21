import { mongoBot } from '$lib/server/mongoBot';
import type { LayoutServerLoad } from './$types';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';
import { ObjectId } from 'mongodb';

export const load: LayoutServerLoad = async (event) => {
	const token = event.cookies.get('session') ?? null;

	if (token === null) {
		return utilityServer.redirectToLogin(event.route);
	}

	const session = await sessionServer.validateSessionToken(token);

	if (session === null) {
		sessionServer.deleteSessionTokenCookie(event);
		return utilityServer.redirectToLogin(event.route);
	}

	const foundUser = await mongoBot.MONGODB_C_AUTH_USERS.FindOne({ _id: { $eq: new ObjectId(session.UserId) } });

	if (foundUser === null) {
		// TODO: log that forgery was successfully made, as the user couldnt be found, but the session was validated.
		return utilityServer.redirectToLogin(event.route);
	}

	sessionServer.setSessionTokenCookie(event, token, session.ExpiresAt);

	return {
		title: 'Skyblock',
		description: 'Edit your Hypixel Skyblock details! Currently contains features for the Auction and Bazaar.'
	};
};
