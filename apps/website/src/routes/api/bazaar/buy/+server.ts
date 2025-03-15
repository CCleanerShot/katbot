import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ cookies, route, url }) => {
	const { response, statusCode, params } = API_CONTRACTS['GET=>/api/bazaar/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;

	if (userId === null) {
		return utilityServer.redirectToLogin(route);
	}

	const mongoResponse = await mongoBot.MONGODB_C_BAZAAR_BUY.Find({ UserId: userId });
	return json({ data: mongoResponse } as typeof response, { status: statusCode });
};

export const DELETE: RequestHandler = async ({ cookies, request, url }) => {
	const { params, response, statusCode } = API_CONTRACTS['DELETE=>/api/bazaar/buy'];
	const { Name } = (await request.json()) as typeof params;

	const userId = BigInt(cookies.get('discordId') as any)!;
	const existingItem = await mongoBot.MONGODB_C_BAZAAR_BUY.FindOne({ Name: Name, UserId: userId });

	if (existingItem !== null) {
		await mongoBot.MONGODB_C_BAZAAR_BUY.DeleteOne({ Name: Name, UserId: userId });
	}

	return json('' as typeof response, { status: statusCode });
};

export const POST: RequestHandler = async ({ cookies, request, route }) => {
	const { item } = (await request.json()) as typeof params;
	const userId = BigInt(cookies.get('discordId') as any)!;

	const { params, response, statusCode } = API_CONTRACTS['POST=>/api/bazaar/buy'];

	const alreadyExists = await mongoBot.MONGODB_C_BAZAAR_BUY.FindOne({ ID: item.ID, UserId: userId });

	if (alreadyExists) {
		await mongoBot.MONGODB_C_BAZAAR_BUY.UpdateOne({ ID: item.ID, UserId: userId }, { $set: { ...item } });
	} else {
		await mongoBot.MONGODB_C_BAZAAR_BUY.InsertOne({ ...item, UserId: userId });
	}

	return json('' as typeof response, { status: statusCode });
};
