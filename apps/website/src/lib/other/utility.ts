import { PUBLIC_DISCORD_OAUTH_REDIRECT_URI, PUBLIC_DISCORD_OAUTH_REDIRECT_URI_TEST, PUBLIC_ENVIRONMENT } from '$env/static/public';
import { API_CONTRACTS } from '$lib/common/apiContracts';

type ClientResponse<T extends any> = Omit<Response, 'json'> & JSON<T>;
type JSON<T> = { /** same as json(), but exists for intellisense */ JSON: () => Promise<T> };

export const utility = {
	betterEntries: <T extends Record<string, any>>(obj: T) => {
		type Entries<T> = { [K in keyof T]: [K, T[K]] }[keyof T][];
		return Object.entries(obj) as Entries<T>;
	},
	capitalize: (input: string): string => {
		return input[0].toUpperCase() + input.slice(1);
	},
	fetch: async <T extends keyof typeof API_CONTRACTS, K extends (typeof API_CONTRACTS)[T]['response']>(
		request: T,
		params: (typeof API_CONTRACTS)[T]['params']
	): Promise<ClientResponse<K>> => {
		const { method, route } = API_CONTRACTS[request];
		const requestInit: RequestInit = { headers: { 'Content-Type': 'application/json' }, method };
		let finalRoute: string = route;
		let result: Response;

		if (method == 'GET') {
			const kvPairs = Object.entries(params);

			if (kvPairs.length > 0) {
				finalRoute += '?';
			}

			for (const [key, value] of kvPairs) {
				finalRoute += `${key}=${value}&`;
			}

			if (finalRoute[finalRoute.length - 1] === '&') {
				finalRoute = finalRoute.slice(0, finalRoute.length - 1);
			}
		} else {
			requestInit.body = JSON.stringify(params);
		}

		result = await fetch(finalRoute, requestInit);
		(result as any).JSON = result.json;
		return result as any as ClientResponse<K>;
	},
	formatNumber: (input: number | bigint): string => {
		const results = utility.reverseString(input.toString()).split(/(.{3})/g);
		return utility.reverseString(results.join(' '));
	},
	getProtocol: (type: 'http' | 'ws'): string => {
		switch (type) {
			case 'http':
				if (PUBLIC_ENVIRONMENT === 'production') {
					return 'https';
				} else {
					return 'http';
				}
			case 'ws':
				if (PUBLIC_ENVIRONMENT === 'production') {
					return 'wss';
				} else {
					return 'ws';
				}
		}
	},
	getRedirectUri(provider: 'discord') {
		switch (provider) {
			case 'discord':
				if (PUBLIC_ENVIRONMENT === 'production') {
					return PUBLIC_DISCORD_OAUTH_REDIRECT_URI;
				} else {
					return PUBLIC_DISCORD_OAUTH_REDIRECT_URI_TEST;
				}
			default:
				throw new Error('Unimplemented provider.');
		}
	},
	randomNumber: (min: number, max: number): number => {
		return min + Math.round(Math.random() * (max - min));
	},
	/** NOTE: returns a new array */
	reverseArray: <T extends any>(input: T[]): T[] => {
		const array = [];

		for (let i = input.length - 1; i >= 0; i--) {
			array.push(input[i]);
		}

		return array;
	},
	reverseString: (input: string): string => {
		const array = [];

		for (let i = input.length - 1; i >= 0; i--) {
			array.push(input[i]);
		}

		return array.join('');
	},
	sleep: async (milliseconds: number) => {
		return new Promise((res, rej) => setTimeout(() => res(true), milliseconds));
	}
} as const;
