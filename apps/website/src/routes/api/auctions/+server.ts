import { API_CONTRACTS } from '$lib/common/apiContracts';
import { mongoBot } from '$lib/server/mongoBot';
import { json, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ url }) => {
	const { response, route } = API_CONTRACTS['GET=>/api/auctions'];
	const items = await mongoBot.MONGODB_C_AUCTION_ITEMS.Find();
	const tags = await mongoBot.MONGODB_C_AUCTION_TAGS.Find();
	return json({ data: { items: items, tags: tags } } as typeof response);
};
