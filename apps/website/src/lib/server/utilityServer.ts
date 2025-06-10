import { LogLevel } from '$lib/enums';
import { error, redirect } from '@sveltejs/kit';

export const utilityServer = {
	errorInvalidCredentials: () => {
		return error(401, 'Invalid credentials.');
	},
	errorNotFound: () => {
		return error(404, 'Not found.');
	},
	logServer: (logLevel: LogLevel, ...data: any[]) => {
		let prefix;

		switch (logLevel) {
			case LogLevel.NONE:
				prefix = '>';
				break;
			case LogLevel.WARN:
				prefix = '?';
				break;
			case LogLevel.ERROR:
				prefix = '!';
				break;
		}

		console.log(`${prefix}:`, ...data);
	},
	redirectToLogin: (route: { id: any }) => {
		const redirectUrl = `/login?redirect=${route.id}`;
		return redirect(307, redirectUrl);
	}
};
