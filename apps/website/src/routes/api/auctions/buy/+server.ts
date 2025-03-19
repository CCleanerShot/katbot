import type { AuctionItem } from '$lib/mongodb/AuctionItem';
import { API_CONTRACTS } from '$lib/other/apiContracts';
import { mongoBot } from '$lib/server/mongoBot';
import { error, json, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ cookies, request }) => {
	const { response, route } = API_CONTRACTS['GET=>/api/auctions/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;
	const items = await mongoBot.MONGODB_C_AUCTION_BUY.Find({ UserId: userId });
	return json({ data: items } as typeof response);
};

export const DELETE: RequestHandler = async ({ cookies, request }) => {
	const { params } = API_CONTRACTS['DELETE=>/api/auctions/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;
	const { ID } = (await request.json()) as typeof params;
	const foundItem = await mongoBot.MONGODB_C_AUCTION_BUY.DeleteOne({ ID: ID, UserId: userId });

	if (foundItem.deletedCount === 0) {
		// TODO: server log that there was the item deleted, yet an attempt to delete it was made! Can be from:
		// attempted forgery
		// race condition
	}

	return json('');
};

export const PUT: RequestHandler = async ({ cookies, request }) => {
	const { params } = API_CONTRACTS['PUT=>/api/auctions/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;
	const { items } = (await request.json()) as typeof params;

	for (const item of items) {
		for (const tag of item.AuctionTags ?? []) {
			const validTag = await mongoBot.MONGODB_C_AUCTION_TAGS.FindOne({ Name: tag.Name, Type: tag.Type });

			if (!validTag) {
				// TODO: server log for bad tags
				return error(501, 'Bad Request');
			}

			const itemHasTag = await mongoBot.MONGODB_C_AUCTION_ITEMS.FindOne({ AuctionTags: 'attributes' });

			if (!itemHasTag) {
				return error(501, 'Bad Request');
			}
		}
	}

	const toInsert: (typeof params)['items'] = [];
	for (const item of items) {
		const exists = await mongoBot.MONGODB_C_AUCTION_BUY.UpsertOne(
			{ ID: item.ID, UserId: userId },
			{ $set: { ...item, UserId: userId } },
			{}
		);

		if (!exists) {
			toInsert.push(item);
		}
	}

	const insertItems: AuctionItem[] = toInsert.map((e) => ({ ...e, UserId: userId, AuctionTags: e.AuctionTags ?? [] }) as AuctionItem);
	const response = await mongoBot.MONGODB_C_AUCTION_BUY.InsertMany(insertItems);

	return json('');
};
