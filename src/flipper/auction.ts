import nbt from "prismarine-nbt";
import { BotEnvironment } from "../environment";
import { utils } from "../utils";
import { supabaseClient } from "../supabase/client";
import { FinishedAuctions, HypixelAuctionItem, OngoingAuctionItem, OngoingAuctions } from "../classes";
import { Client, TextChannel } from "discord.js";

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
			[[], []] as string[][],
		);

		const pricesResponse = await supabaseClient.client
			.from("auction_items")
			.select(`
			*,
			auction_prices ( * )
		`)
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
			{} as Record<string, OngoingAuctionItem>,
		);

		const returnRate = 100000;

		let discordResponse = "";
		for (const item of pricesResponse.data) {
			if (item.auction_prices.length === 0) {
				continue;
			}

			// should work, if an error, client fetch is altered/wrong, or dict creation is wrong
			const priceItem = resultsDict[item.name];

			if (priceItem.highest_bid_amount + returnRate < item.auction_prices[0].average_price) {
				discordResponse += `(Item Name, Rarity, Auction, Average) ${priceItem.item_name} | ${priceItem.tier} | ${priceItem.highest_bid_amount} | ${item.auction_prices[0].average_price}`;
				discordResponse += "\n";
			}
		}

		const textChannel = (await client.channels.fetch(BotEnvironment.DISCORD_SEND_CHANNEL_ID)) as TextChannel;
		await utils.SendBulkText(textChannel, discordResponse);
	}

	async GetOngoingAuctions(): Promise<OngoingAuctionItem[]> {
		const ROUTE = "/v2/skyblock/auctions";

		const amountOfPages = 1;

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
			await utils.Sleep(10);

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
			const data = Buffer.from(auction.item_bytes, "base64");
			const nestedData = ((await nbt.parse(data)).parsed.value as any)!.i!.value!.value![0].tag.value;

			const bin: boolean = auction.bin;
			const price: number = auction.price;

			// either its an outlier sell, or the item is so worthless it doesnt matter
			if (price < 200000) {
				continue;
			}

			// only bins for now
			if (bin === false) {
				continue;
			}

			const name: string = utils.RemoveSpecialText(nestedData.display.value.Name.value);
			const lore: string = nestedData.display.value.Lore.value.value;

			// the last item in the array contains the rarity and item type
			const lastLine = utils.RemoveSpecialText(lore[lore.length - 1]).trim();

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
}

export const hypixelController = new HypixelController();
