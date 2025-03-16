import { API_CONTRACTS } from '$lib/other/apiContracts';
import { mongoBot } from '$lib/server/mongoBot';
import { json, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ cookies, url }) => {
	const { response, route, statusCode } = API_CONTRACTS['GET=>/api/auctions/buy'];
	const userId = BigInt(cookies.get('discordId') as any)!;
	const items = await mongoBot.MONGODB_C_AUCTION_BUY.Find({ UserId: userId });
	return json({ data: items } as typeof response, { status: statusCode });
};
