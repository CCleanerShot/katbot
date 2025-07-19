import { utility } from '$lib/utility/utility';
import { mongoBot } from '$lib/server/mongoBot';
import { type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';
import { DISCORD_OAUTH_CLIENT_SECRET } from '$env/static/private';
import { PUBLIC_DISCORD_OAUTH_CLIENT_ID } from '$env/static/public';
import type { OAuthTokenHeaders, OAuthTokenParams, OAuthTokenResponse } from '$lib/types';

type DiscordUserResponse = {
	id: bigint;
	username: string;
	avatar: string;
	discriminator: string;
	public_flags: number;
	flags: number;
	banner: any;
	accent_color: any;
	global_name: string;
	avatar_decoration_data: any;
	collectibles: any;
	display_name_styles: any;
	banner_color: any;
	clan: any;
	primary_guild: any;
	mfa_enabled: boolean;
	locale: string;
	premium_type: number;
};

// TODO: attach errors on cookies and when redirected back, and have the pages emit toasts from the given errors
export const GET: RequestHandler = async ({ url }) => {
	const code = url.searchParams.get('code');

	if (!code) {
		// TODO: add internal server error notice, based on potential forgery
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
		// TODO: add internal server error notice, based on likely invalid parameters or potential forgery
		return utilityServer.redirectToLogin();
	}

	const tokenBody = _tokenBody as OAuthTokenResponse;
	const userUrl = 'https://discord.com/api/users/@me';
	const userResponse = await fetch(userUrl, { headers: { Authorization: `Bearer ${tokenBody.access_token}1` } });

	if (userResponse.status >= 300 && userResponse.status < 200) {
		// TODO: add internal server error notice, based on likely invalid parameters
		return utilityServer.redirectToLogin();
	}

	const { avatar, id, username }: DiscordUserResponse = await userResponse.json();
	const existingUser = await mongoBot.MONGODB_C_AUTH_DISCORD.FindOne({ UserId: id });

	if (existingUser) {
		// just update details if needed
		await mongoBot.MONGODB_C_AUTH_DISCORD.UpdateOne({ UserId: existingUser.UserId }, { $set: { Avatar: avatar, Username: username } });
	} else {
		const newUser = await mongoBot.MONGODB_C_AUTH_DISCORD.InsertOne({ Avatar: avatar, UserId: id, Username: username });
		const newAuth = await mongoBot.MONGODB_C_AUTH_USERS.InsertOne({ AuthorizationId: newUser.insertedId, AuthorizationSource: 'discord' });

		// TODO: add internal server error notice
		if (!newAuth.acknowledged) {
			return utilityServer.redirectToLogin();
		}
	}

	return new Response('s');
};
