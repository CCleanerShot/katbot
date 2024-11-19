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
		for (let i = 0; i < 5; i++) { // Adjust number of pages if needed
			try {
				const fetchedResults = await fetch(BASE_URL + ROUTE, { headers: HEADERS, method: "GET" });
				const results = (await fetchedResults.json()) as RequestTypes.AuctionEnded;
	
				for (const auction of results.auctions) {
					try {
						const data = Buffer.from(auction.item_bytes, "base64");
						const { parsed } = await nbt.parse(data);
	
						const itemName = (parsed.value as any)?.i?.value?.value[0]?.tag?.value?.display?.value?.Name?.value;
						const lore = (parsed.value as any)?.i?.value?.value[0]?.tag?.value?.display?.value?.Lore?.value?.value;
	
						console.log(`Parsed Item: ${itemName}`, lore);
	
						allResults.push({
							auction_id: auction.auction_id,
							seller: auction.seller,
							seller_profile: auction.seller_profile,
							buyer: auction.buyer,
							buyer_profile: auction.buyer_profile,
							timestamp: auction.timestamp,
							price: auction.price,
							bin: auction.bin,
							item_bytes: auction.item_bytes,
							item_name: itemName,
						});
					} catch (doomed) {
						console.error("Failed to parse item bytes:", auction.item_bytes, doomed);
					}
				}
	
				await CustomUtils.Sleep(100);
			} catch (doomed) {
				console.error("Failed to fetch auctions:", doomed);
			}
		}
	
		// Sort by price and log the top 100 results
		const sorted = allResults.sort((a, b) => b.price - a.price);
		sorted.slice(0, 100).forEach((auction) => {
			console.log(auction.item_name, auction.price, new Date(auction.timestamp));
		});
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
	

	//static async getItemPrices(itemName: string) {
	//	const ROUTE = "/v2/skyblock/auctions_ended";
	//
	//	const allResults: RequestTypes.AuctionEndedItem[] = [];
	//	for (let i = 0; i < 5; i++) { // Fetch up to 5 pages of completed auctions
	//		const fetchedResults = await fetch(BASE_URL + ROUTE, { headers: HEADERS, method: "GET" });
	//		const results = (await fetchedResults.json()) as RequestTypes.AuctionEnded;
	//
	//		for (const auction of results.auctions) {
	//			const data = Buffer.from(auction.item_bytes, "base64");
	//			const { parsed } = await nbt.parse(data);
	//
	//			const parsedName = (parsed.value as any)!.i!.value!.value![0].tag.value.display.value.Name.value;
	//
	//			if (parsedName === itemName) {
	//				allResults.push({
	//					item_name: parsedName,
	//					highest_bid_amount: auction.highest_bid_amount,
	//					end: auction.end,
	//					starting_bid: auction.starting_bid
	//				});
	//			}
	//		}
	//
	//		await CustomUtils.Sleep(100);
	//	}
	//
	//	// Analyze prices
	//	const prices = allResults.map(a => a.highest_bid_amount);
	//	const averagePrice = prices.reduce((sum, price) => sum + price, 0) / prices.length;
	//	const minPrice = Math.min(...prices);
	//	const maxPrice = Math.max(...prices);
	//
	//	console.log(`Prices for ${itemName}:\nAverage: ${averagePrice}\nMin: ${minPrice}\nMax: ${maxPrice}`);
	//	return { averagePrice, minPrice, maxPrice };
	//}
	
}
