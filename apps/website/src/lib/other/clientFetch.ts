import { fetchState } from '$lib/states/fetchState.svelte';
import { toastActions } from '$lib/states/toastsState.svelte';
import { json } from '@sveltejs/kit';
import { API_CONTRACTS } from './apiContracts';

type ClientResponse<T extends any> = Omit<Response, 'json'> & JSON<T>;
type JSON<T> = {
	/** It's the exact same as json(), but exists to circumvent the existing json() from overriding the intended intellisense */
	JSON: () => Promise<T>;
};

/** Wrapper function for `fetch()`. Includes a wrapper `JSON()` method for intellisense.  Can also emits a toast to the page, from the optional parameter, `useToast`. */
export const clientFetch = async <T extends keyof typeof API_CONTRACTS, K extends (typeof API_CONTRACTS)[T]['response']>(
	request: T,
	params: (typeof API_CONTRACTS)[T]['params'],
	useToast: boolean = false
): Promise<ClientResponse<K>> => {
	const { method, route } = API_CONTRACTS[request];
	const requestInit: RequestInit = { headers: { 'Content-Type': 'application/json' }, method };
	let finalRoute: string = route;
	let result: Response;

	fetchState.status = 'loading';

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

	if (useToast) {
		switch (result.ok) {
			case true:
				toastActions.addToast({ message: 'Success!', type: 'NONE' });
				fetchState.status = 'none';
				break;
			case false:
				let message;

				try {
					message = ((await result.json()) as { message: string }).message;

					if (message.includes('Internal Error')) {
						message = 'Server unexpected failed! Contact me if the issue persists.';
					}
				} catch (err) {
					message = 'Server unexpected failed! Contact me if the issue persists.';
				}

				toastActions.addToast({ message: message, type: 'ERROR' });
				fetchState.status = 'error';

				// TODO: get delay from config
				setTimeout(() => {
					fetchState.status = 'none';
				}, 4000);

				break;
		}
	}

	fetchState.status = 'none';

	return result as any as ClientResponse<K>;
};
