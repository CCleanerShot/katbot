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
				const timeLeft = cV.end - Date.now();

				if (timeLeft < 0) {
					return pV;
				}

				if (timeLeft > 60000 * myConfig.MINIMUM_MINUTES_FOR_SALE) {
					return pV;
				}

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

		let discordResponse = "(Item Name, Rarity, Auction, Average, Ending)\n";
		const originalLength = discordResponse.length;

		for (const item of pricesResponse.data) {
			if (item.auction_prices.length === 0) {
				continue;
			}

			// should work, if an error, client fetch is altered/wrong, or dict creation is wrong
			const { end, item_bytes, highest_bid_amount, item_name, tier } = resultsDict[item.name];

			const filterPrice = myConfig.MINIMUM_PRICE_FOR_SALE;
			const avgPrice = item.auction_prices[0].average_price;
			const secondsLeft = (end - Date.now()) / 1000;
			const { Count, Damage, id, tag } = await myUtils.NBTParse(item_bytes);

			console.log(highest_bid_amount, Count.value, filterPrice, avgPrice);

			if (highest_bid_amount / Count.value + filterPrice < avgPrice) {
				discordResponse += `${item_name} **|** ${tier} **|** ${highest_bid_amount} **|** ${avgPrice} **|** ${secondsLeft / 60}m`;
				discordResponse += "\n";
			}
		}

		const textChannel = (await client.channels.fetch(BotEnvironment.DISCORD_SEND_CHANNEL_ID)) as TextChannel;
		if (discordResponse.length === originalLength) {
			await textChannel.send("no sales lol");
		} else {
			await myUtils.SendBulkText(textChannel, discordResponse);
		}
	}

	async GetOngoingAuctions(): Promise<OngoingAuctionItem[]> {
		const ROUTE = "/v2/skyblock/auctions";

		const pages = Array(myConfig.NUMBER_OF_FETCHED_PAGES)
			.fill(null)
			.map((v, i) => {
				return new Promise(async (res, rej) => {
					const query = `?page=${i}`;
					const fetchedResults = await fetch(BASE_URL + ROUTE + query, {
						headers: HEADERS,
						method: "GET",
					});
					const results = new OngoingAuctions(await fetchedResults.json());
					console.log(`done with page ${i}: ${results.auctions.length} results`);

					res(results.auctions);
				}) as Promise<OngoingAuctionItem[]>;
			});

		const allResults = await Promise.allSettled(pages);
		const finalResults: OngoingAuctionItem[] = [];

		for (const result of allResults) {
			if (result.status === "fulfilled") {
				finalResults.push(...result.value);
				continue;
			}

			console.log("failed to fetch, likely failed URL fetch");
		}

		return finalResults;
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
				})
			);
		}

		response.parsedData = results;
		return response;
	}
}

export const hypixelController = new HypixelController();
