import { bazaarSellClient } from '$lib/mongodb/BazaarSell/client';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ url }) => {
	const userId = BigInt(url.searchParams.get('userId')!);
	const { response, route, statusCode } = API_CONTRACTS['GET=>/api/bazaar/sell'];
	// TODO: check if mongo response is ok

	const mongoResponse = bazaarSellClient.find({ UserId: userId });
	const data: typeof response = { data: await mongoResponse };

	return json(data, { status: statusCode });
};
