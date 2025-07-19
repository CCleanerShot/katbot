import { utility } from '$lib/utility/utility';
import { type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';
import { DISCORD_OAUTH_CLIENT_SECRET } from '$env/static/private';
import { PUBLIC_DISCORD_OAUTH_CLIENT_ID } from '$env/static/public';
import type { DiscordUserResponse, OAuthTokenHeadersDiscord, OAuthTokenParamsDiscord, OAuthTokenResponse } from '$lib/types';

// TODO: attach errors on cookies and when redirected back, and have the pages emit toasts from the given errors
export const GET: RequestHandler = async ({ url }) => {
	const code = url.searchParams.get('code');

	if (!code) {
		return utilityServer.redirectToLogin();
	}

	const tokenUrl = new URL('https://discord.com/api/oauth2/token');

	const headers: OAuthTokenHeadersDiscord = {
		'Accept-Encoding': 'application/x-www-form-urlencoded',
		'Content-Type': 'application/x-www-form-urlencoded'
	};

	const params = new URLSearchParams({
		client_id: PUBLIC_DISCORD_OAUTH_CLIENT_ID,
		client_secret: DISCORD_OAUTH_CLIENT_SECRET,
		code: code,
		grant_type: 'authorization_code',
		redirect_uri: utility.getRedirectUri('discord')
	} as OAuthTokenParamsDiscord);

	const tokenResponse = await fetch(tokenUrl, { body: params, headers: headers, method: 'POST' });
	const _tokenBody = await tokenResponse.json();

	if (_tokenBody?.error) {
		return utilityServer.redirectToLogin();
	}

	const tokenBody = _tokenBody as OAuthTokenResponse;

	// const test = mongoBot.MONGODB_C_USERS.FindOne({})

	const userUrl = 'https://discord.com/api/users/@me';

	const userResponse = await fetch(userUrl, { headers: { Authorization: `Bearer ${tokenBody.access_token}1` } });

	if (userResponse.status >= 300 && userResponse.status < 200) {
		return utilityServer.redirectToLogin();
	}

	const userBody: DiscordUserResponse = await userResponse.json();

	// TODO: check existing users from discord atabase if this already exists,
	// if not, create a new discord auth user, then create an authority account to go alongside it
	// if exists, update data according

	console.log(userBody, userResponse.status);
	return new Response('s');
};
