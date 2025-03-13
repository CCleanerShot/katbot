import { mongoBot } from '$lib/mongodb/MongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type Actions, type RequestHandler } from '@sveltejs/kit';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ url }) => {
	console.log('hello');
	const { response, route, statusCode, params } = API_CONTRACTS['GET=>/api/bazaar/sell'];
	const userId = BigInt(url.searchParams.get('userId')!) as typeof params.userId;
	const mongoResponse = await mongoBot.MONGODB_COLLECTION_BAZAAR_SELL.Find({ UserId: userId });
	return json({ data: mongoResponse } as typeof response, { status: statusCode });
};

export const DELETE: RequestHandler = async ({ url }) => {
	const { params, response, route, statusCode } = API_CONTRACTS['DELETE=>/api/bazaar/sell'];
	const name = url.searchParams.get('Name')! as typeof params.Name;
	const mongoResponse = await mongoBot.MONGODB_COLLECTION_BAZAAR_SELL.Find({ Name: name });
	return json('' as typeof response, { status: statusCode });
};

export const POST: RequestHandler = async ({ request, url }) => {
	const data = await request.json();
	console.log(data);
	const { params, response, route, statusCode } = API_CONTRACTS['POST=>/api/bazaar/sell'];
	return json('' as typeof response, { status: statusCode });
};
