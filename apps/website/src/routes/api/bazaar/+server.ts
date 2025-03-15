import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ url }) => {
	const { response, route, statusCode } = API_CONTRACTS['GET=>/api/bazaar'];
	const result = await mongoBot.MONGODB_C_BAZAAR_ITEMS.Find();
	return json({ data: result } as typeof response, { status: statusCode });
};
