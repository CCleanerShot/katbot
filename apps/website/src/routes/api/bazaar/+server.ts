import type { BazaarItemsAll } from '$lib/mongodb/collections/BazaarItemsAll';
import { mongoBot } from '$lib/mongodb/MongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ url }) => {
	const { response, route, statusCode } = API_CONTRACTS['GET=>/api/bazaar'];
	// TODO: check if mongo response is ok

	const result = await mongoBot.MONGODB_COLLECTION_BAZAAR_ITEMS.Find();
	return json({ data: result } as typeof response, { status: statusCode });
};
