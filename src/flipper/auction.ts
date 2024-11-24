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
import { myFetcher } from "../abortController";

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
	cachedAuctions: Record<string, any> = {};
	async GetMoreData() {
		const results = await this.GetFinishedAuctions();
		await results.SaveDataToDatabase();
	}

	async GetGoodSales(client: Client) {
		const allResults = await this.GetOngoingAuctions();
		// likely dont need the end results cuz we'll see them later
		const results = allResults.sort((a, b) => a.end - b.end).slice(0, 10000);

		const resultsDict = results.reduce(
			(pV, cV) => {
				const timeLeft = cV.end - Date.now();

				// ignoring items already checked this session
				if (this.cachedAuctions[cV.uuid]) {
					return pV;
				}

				// if theres barely any time left
				if (timeLeft < 60000 / 4) {
					return pV;
				}

				// if below set auction timer (not taking auctions that are 5hrs away)
				if (cV.bin === false && timeLeft > 60000 * myConfig.data.MINIMUM_MINUTES_FOR_AUCTION_SALE) {
					return pV;
				}

				this.cachedAuctions[cV.uuid] = cV.uuid;
				pV[cV.item_name + cV.tier] = cV;
				return pV;
			},
			{} as Record<string, OngoingAuctionItem>,
		);

		const [names, tiers] = results.reduce(
			(pV, cV) => {
				pV[0].push(cV.item_name); // for some reason, not trimmed
				pV[1].push(cV.tier); // for some reason, not trimmed
				return pV;
			},
			[[], []] as string[][],
		);

		const stack = 500;
		const pricesData: (DB.RowItem & { auction_prices: DB.RowPrice[] })[] = [];

		// splitting it to 20 requests cuz supabase cant handle so many at once
		for (let i = 0; i < 20; i++) {
			const _names = names.slice(stack * i, (i + 1) * stack);
			const _tiers = tiers.slice(stack * i, (i + 1) * stack);

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

			if ((i + 1) * stack > _names.length) {
				break;
			}

			await myUtils.Sleep(10);
		}

		const nameText = "Name";
		const tierText = "Tier";
		const saleText = "Sale";
		const avgText = "Avg";

		const MAX_NAME = 25; // hard coding for simplicity
		const MAX_TIER = 7; // hard coding for simplicity, max is LEGEND-
		const MAX_USER = 10;
		let maxAuction = saleText.length;
		let maxPrice = avgText.length;

		const filterPrice = myConfig.data.MINIMUM_PRICE_FOR_SALE;

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

			if (auction / Count.value + filterPrice > avgPrice) {
				continue;
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

		const titleName = myUtils.SpaceText(MAX_NAME, nameText);
		const titleTier = myUtils.SpaceText(MAX_TIER, tierText);
		const titleAuction = myUtils.SpaceText(maxAuction, saleText);
		const titlePrice = myUtils.SpaceText(maxPrice, avgText);

		let dResponse = `B|${titleName}|${titleTier}|${titleAuction}|${titlePrice}|Ending|User\n`;
		const originalLength = dResponse.length;

		for (const { auction_prices, category, created_at, id, name, tier } of pricesData) {
			if (auction_prices.length === 0) {
				continue;
			}

			if (!resultsDict[name + tier]) {
				continue;
			}

			// should work, if an error, client fetch is altered/wrong, or dict creation is wrong
			const { end, bin, item_bytes, starting_bid, highest_bid_amount, auctioneer } = resultsDict[name + tier];
			const auction = Math.max(highest_bid_amount, starting_bid);
			const { Count, Damage, id, tag } = await myUtils.NBTParse(item_bytes);
			const avgPrice = auction_prices[0].average_price;

			if (auction / Count.value + filterPrice > avgPrice) {
				continue;
			}

			const user = await this.GetPlayer(auctioneer);

			if (!user) {
				// continue anyways, idk whats going on
				console.log("somehow no user...", user, auctioneer);
			}

			const minutesLeft = Math.round(((end - Date.now()) / 1000 / 60) * 10) / 10;
			const uName = user?.displayname ?? "N/A";

			const myBin = bin ? "+" : "-";
			const myName = name.length >= MAX_NAME ? name.slice(0, MAX_NAME - 1) + "-" : myUtils.SpaceText(MAX_NAME, name);
			const myTier = tier.length > MAX_TIER ? tier.slice(0, MAX_TIER - 1) + "-" : tier;
			const myBid = myUtils.SpaceText(maxAuction, myUtils.FormatPrice(auction));
			const myPrice = myUtils.SpaceText(maxPrice, myUtils.FormatPrice(avgPrice));
			const myUser = uName.length > MAX_USER ? uName.slice(0, MAX_USER - 1) + "-" : uName;

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

		const fetchedResults = await myFetcher.Fetch(BASE_URL + ROUTE, {
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

		const finalResults: OngoingAuctionItem[] = [];

		for (let i = 0; i < myConfig.data.NUMBER_OF_FETCHED_PAGES; i++) {
			const query = `?page=${i}`;
			const fetchedResults = await myFetcher.Fetch(BASE_URL + ROUTE + query, {
				headers: HEADERS,
				method: "GET",
			});
			const results = new OngoingAuctions(await fetchedResults.json());
			finalResults.push(...results.auctions);

			await myUtils.Sleep(5);
		}

		return finalResults;
	}

	async GetPlayer(player_uuid: string): Promise<GetPlayerItem> {
		const ROUTE = "/v2/player";
		const query = `?uuid=${player_uuid}`;
		const fetchedResults = await myFetcher.Fetch(BASE_URL + ROUTE + query, {
			headers: HEADERS,
			method: "GET",
		});

		const results = new GetPlayerResponse(await fetchedResults.json());

		return results.player;
	}
}

export const hypixelController = new HypixelController();
