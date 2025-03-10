import  { API_CONTRACTS } from './apiContracts';

type ClientResponse<T extends any> = Omit<Response, 'json'> & JSON<T>;
type JSON<T> = { 
	/** It's the exact same as json(), but exists to circumvent the existing json() from overriding the intended intellisense */
	JSON: () => Promise<T> 
};


/** Wrapper function for `fetch()`. Includes a wrapper `JSON()` method for intellisense.   */
export const clientFetch = async <
	T extends keyof typeof API_CONTRACTS,
	K extends (typeof API_CONTRACTS)[T]['response']
>(
	request: T,
	params: (typeof API_CONTRACTS)[T]['params']
): Promise<ClientResponse<K>> => {
	// TODO: add headers esp. for authorized routes
	const { method, route } = API_CONTRACTS[request];
	const url = new URL(route, window.location.origin);
	let result: Response;

	if (method == 'GET') {
		for (const [key, value] of Object.entries(params)) {
			url.searchParams.append(key, value);
		}

		result = await fetch(url, { method });
	} else {
		result = await fetch(url, { method, body: JSON.stringify(params) });
	}

	(result as any).JSON = result.json;
	return result as any as ClientResponse<K>;
};
