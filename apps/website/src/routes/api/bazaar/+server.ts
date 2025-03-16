import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ url }) => {
	const { response, route } = API_CONTRACTS['GET=>/api/bazaar'];
	const result = await mongoBot.MONGODB_C_BAZAAR_ITEMS.Find();
	return json({ data: result } as typeof response);
};
