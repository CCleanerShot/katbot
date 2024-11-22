import { BotEnvironment } from "../environment";
import { myUtils } from "../utils";
import { supabaseClient } from "../supabase/client";
import { FinishedAuctions, HypixelAuctionItem, OngoingAuctionItem, OngoingAuctions } from "../classes";
import { Client, TextChannel } from "discord.js";
import { myConfig } from "../config";

const DEV_URL = "https://developer.hypixel.net";
const BASE_URL = "https://api.hypixel.net";
const HEADERS = new Headers();

// https://api.hypixel.net/#section/Authentication
HEADERS.append("API-Key", BotEnvironment.HYPIXEL_BOT_KEY);

class HypixelController {
	async GetMoreData() {
		const results = await this.GetFinishedAuctions();
		await results.SaveDataToDatabase();
	}

	async GetGoodSales(client: Client) {
		const allResults = await this.GetOngoingAuctions();
		// likely dont need the end results cuz we'll see them later
		const results = allResults.sort((a, b) => a.end - b.end).slice(0, 100);

		const [names, tiers] = results.reduce(
			(pV, cV) => {
				pV[0].push(cV.item_name.trim()); // for some reason, not trimmed
				pV[1].push(cV.tier.trim()); // for some reason, not trimmed
				return pV;
			},
			[[], []] as string[][]
		);

		const pricesResponse = await supabaseClient.client
			.from("auction_items")
			.select(
				`
			*,
			auction_prices ( * )
		`
			)
			.in("name", names)
			.in("tier", tiers);

		if (pricesResponse.error) {
			console.log("ERROR AT PRICES RESPONSE", pricesResponse.error);
			return;
		}

		const resultsDict = results.reduce(
			(pV, cV) => {
				pV[cV.item_name] = cV;
				return pV;
			},
			{} as Record<string, OngoingAuctionItem>
		);

		let discordResponse = "";
		for (const item of pricesResponse.data) {
			if (item.auction_prices.length === 0) {
				continue;
			}

			// should work, if an error, client fetch is altered/wrong, or dict creation is wrong
			const pItem = resultsDict[item.name];

			const filterPrice = myConfig.MINIMUM_PRICE_FOR_SALE;
			const avgPrice = item.auction_prices[0].average_price;
			const { Count, Damage, id, tag } = await myUtils.NBTParse(pItem.item_bytes);

			if (pItem.highest_bid_amount / Count + filterPrice < avgPrice) {
				discordResponse += `(Item Name, Rarity, Auction, Average) ${pItem.item_name} | ${pItem.tier} | ${pItem.highest_bid_amount} | ${avgPrice}`;
				discordResponse += "\n";
			}
		}

		const textChannel = (await client.channels.fetch(BotEnvironment.DISCORD_SEND_CHANNEL_ID)) as TextChannel;
		await myUtils.SendBulkText(textChannel, discordResponse);
	}

	async GetOngoingAuctions(): Promise<OngoingAuctionItem[]> {
		const ROUTE = "/v2/skyblock/auctions";

		const amountOfPages = 40;

		const allResults: OngoingAuctionItem[] = [];
		for (let i = 0; i < amountOfPages; i++) {
			const query = `?page=${i}`;
			const fetchedResults = await fetch(BASE_URL + ROUTE + query, {
				headers: HEADERS,
				method: "GET",
			});
			const results = new OngoingAuctions(await fetchedResults.json());
			allResults.push(...results.auctions);
			console.log(`done with page ${i}: ${results.auctions.length} results`);
			await myUtils.Sleep(10);

			// meaning we are at the end of the pages
			if (results.auctions.length < 1000) break;
		}

		return allResults;
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
			const price: number = auction.price / Count.value; // consider it 1 purchase

			// either its an outlier sell, or the item is so worthless it doesnt matter
			if (price < myConfig.MINIMUM_PRICE_TO_IGNORE_SAVE) {
				continue;
			}

			// only bins for now
			if (bin === false) {
				continue;
			}

			const name: string = myUtils.RemoveSpecialText(tag.display.value.Name.value);
			const lore: string = tag.display.value.Lore.value.value;

			// console.log("- - - - - - - - - - - - -");

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
				})
			);
		}

		response.parsedData = results;
		return response;
	}
}

export const hypixelController = new HypixelController();
