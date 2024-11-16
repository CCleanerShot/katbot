import nbt from "prismarine-nbt";
import { BotEnvironment } from "../environments";
import { RequestTypes } from "../types";
import { CustomUtils } from "../utils";
import e from "express";

const DEV_URL = "https://developer.hypixel.net";
const BASE_URL = "https://api.hypixel.net";
// headers
const HEADERS = new Headers();
// https://api.hypixel.net/#section/Authentication
HEADERS.append("API-Key", BotEnvironment.HYPIXEL_BOT_KEY);

export class HypixelRoutes {
	static async getAuctions() {
		const ROUTE = "/v2/skyblock/auctions";

		const amountOfPages = 1;

		const allResults: RequestTypes.AuctionItem[] = [];
		for (let i = 0; i < amountOfPages; i++) {
			const query = `?page=${i}`;
			const fetchedResults = await fetch(BASE_URL + ROUTE + query, { headers: HEADERS, method: "GET" });
			const results = (await fetchedResults.json()) as RequestTypes.Auctions;
			allResults.push(...results.auctions);
			console.log(`done with page ${i}: ${results.auctions.length} results`);
			await CustomUtils.Sleep(100);

			// meaning we are at the end of the pages
			if (results.auctions.length < 1000) break;
		}

		const sorted = allResults.sort((a, b) => a.end - b.end);
		const currentTime = new Date().getTime();

		sorted.forEach((a, i) => {
			if (i > 100) return;

			console.log(a.item_name, a.highest_bid_amount, a.end, a.starting_bid, currentTime - a.end);
		});

		const data = CustomUtils.ReadAuctionData();
		data.push(...allResults);
		CustomUtils.WriteAuctionData(data);
	}

	static async getFinishedAuctions() {
		const ROUTE = "/v2/skyblock/auctions_ended";

		const amountOfPages = 1;

		const allResults: RequestTypes.AuctionEndedItem[] = [];
		for (let i = 0; i < amountOfPages; i++) {
			const fetchedResults = await fetch(BASE_URL + ROUTE, { headers: HEADERS, method: "GET" });
			const results = (await fetchedResults.json()) as RequestTypes.AuctionEnded;

			for (let i = 0; i < results.auctions.length; i++) {
				const auction = results.auctions[i];
				if (i > 10) return;

				const data = Buffer.from(auction.item_bytes, "base64");
				const { parsed } = await nbt.parse(data);

				const what = (parsed.value as any)!.i!.value!.value![0].tag.value.display.value.Name.value;
				const the = (parsed.value as any)!.i!.value!.value![0].tag.value.display.value.Lore.value.value;
				const fuck = (parsed.value as any)!.i!.value!.value![0].tag.value;

				console.log(what, the, fuck);
			}
			await CustomUtils.Sleep(100);
		}

		// const sorted = allResults.sort((a, b) => a.end - b.end);
		// const currentTime = new Date().getTime();

		// sorted.forEach((a, i) => {
		// 	if (i > 100) return;

		// 	console.log(a.item_name, a.highest_bid_amount, a.end, a.starting_bid, currentTime - a.end);
		// });

		// const data = CustomUtils.ReadAuctionData();
		// data.push(...allResults);
		// CustomUtils.WriteAuctionData(data);
	}
}
