import { mongoBot } from '$lib/server/mongoBot';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { error, json, type RequestHandler } from '@sveltejs/kit';
import { utilityServer } from '$lib/server/utilityServer';
import { BazaarItem } from '$lib/mongodb/BazaarItem';

// TODO: add middleware that validates the search params
export const GET: RequestHandler = async ({ cookies, route, url }) => {
	const { response, params } = API_CONTRACTS['GET=>/api/bazaar/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;

	if (userId === null) {
		return utilityServer.redirectToLogin(route);
	}

	const mongoResponse = await mongoBot.MONGODB_C_BAZAAR_BUY.Find({ UserId: userId });
	return json({ data: mongoResponse } as typeof response);
};

export const DELETE: RequestHandler = async ({ cookies, request }) => {
	const { params, response } = API_CONTRACTS['DELETE=>/api/bazaar/buy'];
	const { ID } = (await request.json()) as typeof params;

	// check if ID is valid
	const validID = await mongoBot.MONGODB_C_BAZAAR_ITEMS.FindOne({ ID: ID });

	if (!validID) {
		return error(501, 'Bad Request!');
	}

	const userId = BigInt(cookies.get('discordId') as any)!;
	const existingItem = await mongoBot.MONGODB_C_BAZAAR_BUY.FindOne({ ID: ID, UserId: userId });

	if (existingItem !== null) {
		await mongoBot.MONGODB_C_BAZAAR_BUY.DeleteOne({ ID: ID, UserId: userId });
	}

	return json('' as typeof response);
};

export const PUT: RequestHandler = async ({ request, cookies }) => {
	const { params, response } = API_CONTRACTS['PUT=>/api/bazaar/buy'];
	const { items } = (await request.json()) as typeof params;
	const userId = BigInt(cookies.get('discordId') as any)!;
	const toInsert: typeof items = [];

	for (const item of items) {
		// check if ID is valid
		const validID = await mongoBot.MONGODB_C_BAZAAR_ITEMS.FindOne({ ID: item.ID });

		if (!validID) {
			return error(501, 'Bad Request!');
		}

		const exists = await mongoBot.MONGODB_C_BAZAAR_BUY.UpsertOne(
			{ Name: item.Name, UserId: userId },
			{ $set: { ...item, UserId: userId } },
			{}
		);

		if (exists) {
			continue;
		}

		toInsert.push(item);
	}

	if (toInsert.length) {
		await mongoBot.MONGODB_C_BAZAAR_BUY.InsertMany(toInsert.map((e) => ({ ...e, UserId: userId }) as BazaarItem));
	}

	return json('' as typeof response);
};
