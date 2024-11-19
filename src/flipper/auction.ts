import nbt from "prismarine-nbt";
import { BotEnvironment } from "../environment";
import { utils } from "../utils";
import { FinishedAuctions, OngoingAuctionItem, OngoingAuctions } from "../classes";


const DEV_URL = "https://developer.hypixel.net";
const BASE_URL = "https://api.hypixel.net";
const HEADERS = new Headers();

// https://api.hypixel.net/#section/Authentication
HEADERS.append("API-Key", BotEnvironment.HYPIXEL_BOT_KEY);

export class HypixelController {
	async GetOngoingAuctions() {
		const ROUTE = "/v2/skyblock/auctions";

		const amountOfPages = 1;

		const allResults: OngoingAuctionItem[] = [];
		for (let i = 0; i < amountOfPages; i++) {
			const query = `?page=${i}`;
			const fetchedResults = await fetch(BASE_URL + ROUTE + query, { headers: HEADERS, method: "GET" });
			const results = new OngoingAuctions(await fetchedResults.json());
			allResults.push(...results.auctions);
			console.log(`done with page ${i}: ${results.auctions.length} results`);
			await utils.Sleep(10);

			// meaning we are at the end of the pages
			if (results.auctions.length < 1000) break;
		}

		const sorted = allResults.sort((a, b) => a.end - b.end);
		const currentTime = new Date().getTime();

		sorted.forEach((a, i) => {
			if (i > 100) return;

			console.log(a.item_name, a.highest_bid_amount, a.end, a.starting_bid, currentTime - a.end);
		});
	}

	async GetFinishedAuctions() {
		const ROUTE = "/v2/skyblock/auctions_ended";

		const fetchedResults = await fetch(BASE_URL + ROUTE, { headers: HEADERS, method: "GET" });
		const results = new FinishedAuctions((await fetchedResults.json()));
		await results.ParseRawData();
	}
}

export const hypixelController = new HypixelController();