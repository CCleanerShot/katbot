import { Environment } from "../environments";
import { RequestTypes } from "../types";

const DEV_URL = "https://developer.hypixel.net";
const BASE_URL = "https://api.hypixel.net";
// headers
const HEADERS = new Headers();
// https://api.hypixel.net/#section/Authentication
HEADERS.append("API-Key", Environment.HYPIXEL_BOT_KEY);

// test body

export async function fetchBazaar() {
	const ROUTE = "/v2/skyblock/auctions";
	const query = await fetch(BASE_URL + ROUTE, { headers: HEADERS, method: "GET" });
	const results = (await query.json()) as RequestTypes.Auctions;
	results.auctions.forEach((a) => {
		console.log(a.item_name, a.highest_bid_amount, a.end);
	});
}
