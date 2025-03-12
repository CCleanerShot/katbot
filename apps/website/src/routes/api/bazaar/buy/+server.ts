import { mongoBot } from '$lib/mongodb/MongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ url }) => {
	const { response, route, statusCode, params } = API_CONTRACTS['GET=>/api/bazaar/buy'];
	const userId = BigInt(url.searchParams.get('userId')!) as typeof params.userId;
	// TODO: check if mongo response is ok
	const mongoResponse = mongoBot.MONGODB_COLLECTION_BAZAAR_BUY.find({UserId: userId})
	const data: typeof response = { data: await mongoResponse.toArray() };

	console.log(data)
	return json(data, { status: statusCode });
};

export const DELETE: RequestHandler = async({url}) => {
	const { response, route, statusCode, params } = API_CONTRACTS['DELETE=>/api/bazaar/buy'];
	const name = url.searchParams.get('Name')! as typeof params.Name;
	// TODO: check if mongo response is ok
	const mongoResponse = mongoBot.MONGODB_COLLECTION_BAZAAR_BUY.find({Name: name})
	return json("" as typeof response, { status: statusCode }); 
}