import { LogLevel } from '$lib/enums';
import { utility } from '$lib/common/utility';
import { mongoBot } from '$lib/server/mongoBot';
import { type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';
import { DISCORD_OAUTH_CLIENT_SECRET } from '$env/static/private';
import { PUBLIC_DISCORD_OAUTH_CLIENT_ID } from '$env/static/public';
import type { OAuthTokenHeaders, OAuthTokenParams, OAuthTokenResponse } from '$lib/types';
import { discordServer } from '$lib/server/discordServer';
import type { API_CONTRACTS } from '$lib/common/apiContracts';

// TODO: attach errors on cookies and when redirected back, and have the pages emit toasts from the given errors
export const GET: RequestHandler = async ({ request, url, fetch }) => {
	const code = url.searchParams.get('code');

	if (!code) {
		// TODO: add error to client
		utilityServer.logServer(LogLevel.WARN, 'Invalid code received from request for OAuth!', code);
		return utilityServer.redirectToLogin();
	}

	const tokenUrl = new URL('https://discord.com/api/oauth2/token');
	const headers: OAuthTokenHeaders = {
		'Accept-Encoding': 'application/x-www-form-urlencoded',
		'Content-Type': 'application/x-www-form-urlencoded'
	};

	const params = new URLSearchParams({
		client_id: PUBLIC_DISCORD_OAUTH_CLIENT_ID,
		client_secret: DISCORD_OAUTH_CLIENT_SECRET,
		code: code,
		grant_type: 'authorization_code',
		redirect_uri: utility.getRedirectUri('discord')
	} as OAuthTokenParams);

	const tokenResponse = await fetch(tokenUrl, { body: params, headers: headers, method: 'POST' });
	const _tokenBody = await tokenResponse.json();

	if (_tokenBody?.error) {
		// TODO: add error to client
		utilityServer.logServer(LogLevel.WARN, 'Error received when attempting to receive an OAuth token!', _tokenBody.error);
		return utilityServer.redirectToLogin();
	}

	const { access_token, refresh_token } = _tokenBody as OAuthTokenResponse;
	const json = await discordServer.getMe(access_token);
	const { avatar, id, username: Username } = json;
	const existingUser = await mongoBot.MONGODB_C_AUTH_DISCORD.FindOne({ UserId: id });

	if (existingUser) {
		// just update details if needed
		const existingAuthUser = await mongoBot.MONGODB_C_AUTH_USERS.FindOne({ _id: { $eq: existingUser._id_AuthUser } });

		if (!existingAuthUser) {
			// TODO: add error to client
			utilityServer.logServer(LogLevel.ERROR, 'Found the discord user, but did not find the auth user. Did someone delete it?');
			return utilityServer.redirectToLogin();
		}

		await mongoBot.MONGODB_C_AUTH_DISCORD.UpdateOne({ UserId: existingUser.UserId }, { $set: { Avatar: avatar, Username } });
	} else {
		const newAuth = await mongoBot.MONGODB_C_AUTH_USERS.InsertOne({ Password: refresh_token });
		const newUser = await mongoBot.MONGODB_C_AUTH_DISCORD.InsertOne({
			_id_AuthUser: newAuth.insertedId,
			Avatar: avatar,
			UserId: id,
			Username
		});

		if (!newAuth.acknowledged) {
			// TODO: add error to client
			utilityServer.logServer(LogLevel.WARN, 'Failed to create a new auth user to database!');
			return utilityServer.redirectToLogin();
		}

		if (!newUser.acknowledged) {
			// TODO: add error to client
			utilityServer.logServer(LogLevel.WARN, 'Failed to create a new discord user to database!');
			return utilityServer.redirectToLogin();
		}
	}

	const user: (typeof API_CONTRACTS)['POST=>/api/login']['params'] = { user: { Password: access_token, Provider: 'discord', Username } };
	const login = await utility.fetch('POST=>/api/login', user, fetch);

	const response = await login.JSON();
	console.log(response);
	return new Response('s');
};
