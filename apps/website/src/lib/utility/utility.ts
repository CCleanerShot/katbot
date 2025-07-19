import { PUBLIC_DISCORD_OAUTH_REDIRECT_URI, PUBLIC_DISCORD_OAUTH_REDIRECT_URI_TEST, PUBLIC_ENVIRONMENT } from '$env/static/public';

export const utility = {
	betterEntries: <T extends Record<string, any>>(obj: T) => {
		type Entries<T> = { [K in keyof T]: [K, T[K]] }[keyof T][];
		return Object.entries(obj) as Entries<T>;
	},
	capitalize: (input: string): string => {
		return input[0].toUpperCase() + input.slice(1);
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
