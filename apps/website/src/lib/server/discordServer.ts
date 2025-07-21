import { LogLevel } from '$lib/enums';
import { utilityServer } from './utilityServer';

export const discordServer = {
	/**
	 * Gets the details of the current user, given the access token.
	 * Has optional parameter for replacing the error action (by default: redirects the client to login)
	 */
	async getMe(access_token: string, fail?: () => any): Promise<DiscordUserResponse> {
		const url = 'https://discord.com/api/users/@me';
		const headers: HeadersInit = { Authorization: `Bearer ${access_token}` };
		const response = await fetch(url, { headers });

		if (response.status >= 300 || response.status <= 199) {
			// TODO: add error to client
			utilityServer.logServer(LogLevel.WARN, 'Response Code from Discord with access token was not in the 200 range!', response);

			if (fail) {
				return fail();
			} else {
				return utilityServer.redirectToLogin();
			}
		}

		return await response.json();
	},
	/** regenerates a new access token from given refresh token */
	async getAccessToken(refresh_token: string): Promise<DiscordAccessTokenResponse> {
		const url = 'https://discord.com/api/v10';
		const body = new URLSearchParams({ grant_type: 'refresh_token', refresh_token: refresh_token });
		const headers: HeadersInit = {
			'Accept-Encoding': 'application/x-www-form-urlencoded',
			'Content-Type': 'application/x-www-form-urlencoded'
		};

		const response = await fetch(url, { body, headers, method: 'POST' });
		console.log(response);
		return {};
	}
};

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

type DiscordAccessTokenResponse = {};
