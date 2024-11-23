import { BotEnvironment } from "../environment";
import { myUtils } from "../utils";
import { supabaseClient } from "../supabase/client";
import { FinishedAuctionItem, HypixelAuctionItem, OngoingAuctionItem, OngoingAuctions } from "../classes";
import { Client, TextChannel } from "discord.js";
import { myConfig } from "../config";
import { FinishedAuctions } from "../classes/hypixel_routes/FinishedAuctions";
import { GetPlayerItem } from "../classes/hypixel_routes/GetPlayerItem";
import { GetPlayerResponse } from "../classes/hypixel_routes/GetPlayerResponse";
import { DB } from "../types";

const DEV_URL = "https://developer.hypixel.net";
const BASE_URL = "https://api.hypixel.net";
const HEADERS = new Headers();

// https://api.hypixel.net/#section/Authentication
HEADERS.append("API-Key", BotEnvironment.HYPIXEL_BOT_KEY);

type FinishedAuctionsResponse = {
	success: boolean;
	lastUpdated: number;
	auctions: FinishedAuctionItem[];
	parsedData: HypixelAuctionItem[];
};

class HypixelController {
	async GetMoreData() {
		const results = await this.GetFinishedAuctions();
		await results.SaveDataToDatabase();
	}

	async GetGoodSales(client: Client) {
		const allResults = await this.GetOngoingAuctions();
		// likely dont need the end results cuz we'll see them later
		const results = allResults.sort((a, b) => a.end - b.end).slice(0, 2000);

		const resultsDict = results.reduce(
			(pV, cV) => {
				pV[cV.item_name + cV.tier] = cV;
				return pV;
			},
			{} as Record<string, OngoingAuctionItem>,
		);

		const [names, tiers] = results.reduce(
			(pV, cV) => {
				const timeLeft = cV.end - Date.now();

				if (timeLeft < 60000 / 2) {
					return pV;
				}

				if (timeLeft > 60000 * myConfig.data.MINIMUM_MINUTES_FOR_SALE) {
					return pV;
				}

				pV[0].push(cV.item_name); // for some reason, not trimmed
				pV[1].push(cV.tier); // for some reason, not trimmed
				return pV;
			},
			[[], []] as string[][],
		);

		const stack = 500;
		const pricesData: (DB.RowItem & { auction_prices: DB.RowPrice[] })[] = [];

		// splitting it to 5 requests cuz supabase cant handle so many at once
		for (let i = 0; i < 5; i++) {
			const _names = names.slice(i * stack, i * (stack + 1));
			const _tiers = tiers.slice(i * stack, i * (stack + 1));

			const pricesResponse = await supabaseClient.client
				.from("auction_items")
				.select(`
			*,
			auction_prices ( * )
			`)
				.in("name", _names)
				.in("tier", _tiers);

			if (pricesResponse.error) {
				for (let i = 0; i < names.length; i++) {
					console.log(_names[i], _tiers[i]);
				}

				console.log("ERROR AT PRICES RESPONSE", pricesResponse.error);
				return;
			}

			pricesData.push(...pricesResponse.data);

			if (i * (stack + 1) > _names.length) {
				break;
			}
		}

		const nameText = "Name";
		const tierText = "Tier";
		const saleText = "Sale";
		const avgText = "Avg";
		let maxName = nameText.length;
		let maxTier = tierText.length;
		let maxAuction = saleText.length;
		let maxPrice = avgText.length;

		for (const { auction_prices, category, created_at, id, name, tier } of pricesData) {
			if (auction_prices.length === 0) {
				continue;
			}

			if (!resultsDict[name + tier]) {
				continue;
			}

			const { end, item_bytes, starting_bid, highest_bid_amount, item_name, auctioneer } = resultsDict[name + tier];
			const auction = Math.max(highest_bid_amount, starting_bid);
			const { Count, Damage, id, tag } = await myUtils.NBTParse(item_bytes);
			const avgPrice = auction_prices[0].average_price;
			const filterPrice = myConfig.data.MINIMUM_PRICE_FOR_SALE;

			if (auction / Count.value + filterPrice > avgPrice) {
				continue;
			}

			if (name.length > maxName) {
				maxName = name.length;
			}

			if (tier.length > maxTier) {
				maxTier = tier.length;
			}

			const auctionWithSpaces = myUtils.FormatPrice(auction);
			const priceWithSpaces = myUtils.FormatPrice(avgPrice);

			if (auctionWithSpaces.length > maxAuction) {
				maxAuction = auctionWithSpaces.length;
			}

			if (priceWithSpaces.length > maxPrice) {
				maxPrice = priceWithSpaces.length;
			}
		}

		const titleName = myUtils.SpaceText(maxName, nameText);
		const titleTier = myUtils.SpaceText(maxTier, tierText);
		const titleAuction = myUtils.SpaceText(maxAuction, saleText);
		const titlePrice = myUtils.SpaceText(maxPrice, avgText);

		let dResponse = `BIN|${titleName}|${titleTier}|${titleAuction}|${titlePrice}|Ending|User\n`;
		const originalLength = dResponse.length;

		for (const { auction_prices, category, created_at, id, name, tier } of pricesData) {
			if (auction_prices.length === 0) {
				continue;
			}

			if (!resultsDict[name + tier]) {
				continue;
			}

			// should work, if an error, client fetch is altered/wrong, or dict creation is wrong
			const { end, bin, item_bytes, starting_bid, highest_bid_amount, item_name, auctioneer } =
				resultsDict[name + tier];
			const auction = Math.max(highest_bid_amount, starting_bid);
			const { Count, Damage, id, tag } = await myUtils.NBTParse(item_bytes);
			const avgPrice = auction_prices[0].average_price;
			const filterPrice = myConfig.data.MINIMUM_PRICE_FOR_SALE;

			if (auction / Count.value + filterPrice > avgPrice) {
				continue;
			}

			const user = await this.GetPlayer(auctioneer);

			const minutesLeft = Math.round(((end - Date.now()) / 1000 / 60) * 1000) / 1000;

			const myBin = bin ? "  +" : "  -";
			const myName = myUtils.SpaceText(maxName, item_name);
			const myTier = myUtils.SpaceText(maxTier, tier);
			const myBid = myUtils.SpaceText(maxAuction, myUtils.FormatPrice(auction));
			const myPrice = myUtils.SpaceText(maxPrice, myUtils.FormatPrice(avgPrice));
			const myUser = user.displayname;

			dResponse += `${myBin}|${myName}|${myTier}|${myBid}|${myPrice}|${minutesLeft}m|${myUser}\n`;
		}

		const textChannel = (await client.channels.fetch(BotEnvironment.DISCORD_SEND_CHANNEL_ID)) as TextChannel;

		if (dResponse.length === originalLength) {
			console.log("no sales found!");
		} else {
			await myUtils.SendBulkTextCode(textChannel, dResponse);
		}

		console.log("done fetching sales!");
	}

	async GetFinishedAuctions(): Promise<FinishedAuctions> {
		const ROUTE = "/v2/skyblock/auctions_ended";

		const fetchedResults = await fetch(BASE_URL + ROUTE, {
			headers: HEADERS,
			method: "GET",
		});

		const response = new FinishedAuctions(await fetchedResults.json());

		const results: HypixelAuctionItem[] = [];

		for (let i = 0; i < response.auctions.length; i++) {
			const auction = response.auctions[i];
			const { Count, Damage, id, tag } = await myUtils.NBTParse(auction.item_bytes);

			const bin: boolean = auction.bin;
			const price: number = Math.round(auction.price / Count.value); // consider it 1 purchase

			const name: string = myUtils.RemoveSpecialText(tag.value.display.value.Name.value);
			const lore: string = tag.value.display.value.Lore.value.value;

			// the last item in the array contains the rarity and item type
			const lastLine = myUtils.RemoveSpecialText(lore[lore.length - 1]).trim();

			/// cut off from the first space
			const cutIndex = lastLine.indexOf(" ");

			let rarity = "";
			let category = "";

			if (cutIndex === -1) {
				rarity = lastLine;
			} else {
				category = lastLine.slice(cutIndex);
				rarity = lastLine.slice(0, cutIndex + 1);
			}

			results.push(
				new HypixelAuctionItem({
					bin,
					name,
					category,
					price,
					created_at: auction.timestamp,
					tier: rarity,
				}),
			);
		}

		response.parsedData = results;
		return response;
	}

	private async _ROUTES() {}
	async GetOngoingAuctions(): Promise<OngoingAuctionItem[]> {
		const ROUTE = "/v2/skyblock/auctions";

		// const pages = Array(.data.NUMBER_OF_FETCHED_PAGES)
		// 	.fill(null)
		// 	.map((v, i) => {
		// 		return new Promise(async (res, rej) => {
		// 			const query = `?page=${i}`;
		// 			const fetchedResults = await fetch(BASE_URL + ROUTE + query, {
		// 				headers: HEADERS,
		// 				method: "GET",
		// 			});
		// 			const results = new OngoingAuctions(await fetchedResults.json());
		// 			res(results.auctions);
		// 		}) as Promise<OngoingAuctionItem[]>;
		// 	});

		// const allResults = await Promise.allSettled(pages);

		// const finalResults: OngoingAuctionItem[] = [];

		// for (const result of allResults) {
		// 	if (result.status === "fulfilled") {
		// 		finalResults.push(...result.value);
		// 		continue;
		// 	}

		// 	console.log("failed to fetch, likely failed URL fetch");
		// }

		const finalResults: OngoingAuctionItem[] = [];

		for (let i = 0; i < myConfig.data.NUMBER_OF_FETCHED_PAGES; i++) {
			const query = `?page=${i}`;
			const fetchedResults = await fetch(BASE_URL + ROUTE + query, {
				headers: HEADERS,
				method: "GET",
			});
			const results = new OngoingAuctions(await fetchedResults.json());
			finalResults.push(...results.auctions);

			await myUtils.Sleep(1);
		}

		return finalResults;
	}

	async GetPlayer(player_uuid: string): Promise<GetPlayerItem> {
		const ROUTE = "/v2/player";
		const query = `?uuid=${player_uuid}`;
		const fetchedResults = await fetch(BASE_URL + ROUTE + query, {
			headers: HEADERS,
			method: "GET",
		});

		const results = new GetPlayerResponse(await fetchedResults.json());

		return results.player;
	}
}

export const hypixelController = new HypixelController();
