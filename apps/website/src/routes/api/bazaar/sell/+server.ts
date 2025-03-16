import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { json, type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ cookies, route, url }) => {
	const { response, params } = API_CONTRACTS['GET=>/api/bazaar/sell'];
	const userId = BigInt(cookies.get('discordId') as any)!;

	if (userId === null) {
		return utilityServer.redirectToLogin(route);
	}

	const mongoResponse = await mongoBot.MONGODB_C_BAZAAR_SELL.Find({ UserId: userId });
	return json({ data: mongoResponse } as typeof response);
};

export const DELETE: RequestHandler = async ({ cookies, url }) => {
	const { params, response } = API_CONTRACTS['DELETE=>/api/bazaar/sell'];
	const id = url.searchParams.get('Name')! as typeof params.ID;
	const userId = BigInt(cookies.get('discordId') as any)!;
	const existingItem = await mongoBot.MONGODB_C_BAZAAR_SELL.Find({ ID: id, UserId: userId });

	if (existingItem !== null) {
		await mongoBot.MONGODB_C_BAZAAR_SELL.DeleteOne({ ID: id, UserId: userId });
	}

	return json('' as typeof response);
};

export const POST: RequestHandler = async ({ cookies, request, route }) => {
	const { item } = (await request.json()) as typeof params;
	const userId = BigInt(cookies.get('discordId') as any)!;

	const { params, response } = API_CONTRACTS['POST=>/api/bazaar/sell'];

	const alreadyExists = await mongoBot.MONGODB_C_BAZAAR_SELL.FindOne({ ID: item.ID, UserId: userId });

	if (alreadyExists) {
		await mongoBot.MONGODB_C_BAZAAR_SELL.UpdateOne({ ID: item.ID, UserId: userId }, { $set: { ...item } });
	} else {
		await mongoBot.MONGODB_C_BAZAAR_SELL.InsertOne({ ...item, UserId: userId });
	}

	return json('' as typeof response);
};
