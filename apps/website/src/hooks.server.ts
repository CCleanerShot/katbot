import type { Handle, HandleFetch } from '@sveltejs/kit';
import { sessionServer } from '$lib/server/sessionServer';
import { utilityServer } from '$lib/server/utilityServer';
import { LogLevel } from '$lib/enums';

export const handle: Handle = async ({ event, resolve }) => {
	const url = event.url.toString();
	utilityServer.logServer(LogLevel.NONE, `(${event.request.method}) ${url.toString()}`);

	// chrome dev tools
	if (url.toString().includes('.well-known/appspecific/com.chrome.devtools.json')) {
		return utilityServer.errorNotFound();
	}

	if (!event.url.pathname.includes('api')) {
		const response = await resolve(event, {});
		return response;
	}

	const exceptions = ['/api/login'];

	if (exceptions.includes(event.url.pathname)) {
		const response = await resolve(event, {});
		return response;
	}

	const token = event.cookies.get('session') ?? null;
	const discordId = event.cookies.get('discordId') ?? null;

	if (token === null || discordId === null) {
		return utilityServer.errorInvalidCredentials();
	}

	const session = await sessionServer.validateSessionToken(token);

	if (session === null) {
		return utilityServer.errorInvalidCredentials();
	}

	const response = await resolve(event, {});

	return response;
};
