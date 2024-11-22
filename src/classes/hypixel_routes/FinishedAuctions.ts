import nbt from "prismarine-nbt";
import { myUtils } from "../../utils";
import { FinishedAuctionItem } from "./FinishedAuctionItem";
import { Database } from "../../supabase/types";
import { HypixelAuctionItem } from "./HypixelAuctionItem";
import { supabaseClient } from "../../supabase/client";

/** /auctions_ended */
export class FinishedAuctions {
	success: boolean;
	lastUpdated: number;
	auctions: FinishedAuctionItem[];
	parsedData: HypixelAuctionItem[] = [];
	constructor(params: { success: boolean; lastUpdated: number; auctions: FinishedAuctionItem[] }) {
		this.success = params.success;
		this.lastUpdated = params.lastUpdated;
		this.auctions = params.auctions;
	}

	/** handles adding only new items to the database, as well as adding new prices for each found item */
	public async SaveDataToDatabase() {
		function createDictFromArray<K>(obj: K[], nameLocation: (obj: K) => string): Record<string, K> {
			const result: Record<string, K> = {};

			for (const item of obj) {
				result[nameLocation(item)] = item;
			}

			return result;
		}

		// STEPS:
		// 1. fetch item rows where items match of the same type
		// 2. add new item rows where they dont already exist
		// 3. override price rows where the item + hour exist
		// 4. add new price rows where the item + hour dont exist

		/////////////////////
		// NEW ITEMS START //
		/////////////////////

		// creating dicts to optimize out n^2 operations
		// which gets to trillions easily in our case
		const allItemsDict: Record<string, Database["public"]["Tables"]["auction_items"]["Row"]> = {};
		const itemsToAddDict: Record<string, Database["public"]["Tables"]["auction_items"]["Insert"]> = createDictFromArray(
			this.parsedData,
			(obj) => obj.name
		);

		const currentItemsResponse = await supabaseClient.client
			.from("auction_items")
			.select("*")
			.in(
				"name",
				this.parsedData.map((e) => e.name)
			)
			.in(
				"category",
				this.parsedData.map((e) => e.category)
			)
			.in(
				"tier",
				this.parsedData.map((e) => e.tier)
			);

		if (currentItemsResponse.error) {
			// dont continue to prevent duplicate items
			console.log("ERROR AT CURRENT ITEMS:", currentItemsResponse);
			return;
		}

		for (const existingItem of currentItemsResponse.data) {
			allItemsDict[existingItem.name] = existingItem;
			const potentialItem = { ...itemsToAddDict[existingItem.name] };

			if (potentialItem && potentialItem.tier === existingItem.tier) {
				delete itemsToAddDict[existingItem.name];
			}
		}

		const itemsToAddArray: Database["public"]["Tables"]["auction_items"]["Insert"][] = [];

		for (const key in itemsToAddDict) {
			const { category, created_at, name, tier, id } = itemsToAddDict[key];
			itemsToAddArray.push({ category: category!, created_at: created_at!, name: name!, tier: tier! });
		}

		const addItemsResponse = await supabaseClient.client.from("auction_items").insert(itemsToAddArray).select();

		if (addItemsResponse.error) {
			// dont continue to prevent missing items when adding prices
			console.log("ERROR AT ADDING ITEMS:", addItemsResponse);
			return;
		}

		for (const item of addItemsResponse.data) {
			allItemsDict[item.name] = item;
		}

		///////////////////
		// NEW ITEMS END //
		///////////////////

		//////////////////
		// PRICES START //
		//////////////////
		const existingPricesToOverrideResponse = await supabaseClient.client
			.from("auction_prices")
			.select(
				`
				*,
				auction_items ( * )
			`
			)
			.in("item_id", [...currentItemsResponse.data.map((e) => e.id), ...addItemsResponse.data.map((e) => e.id)]);

		if (existingPricesToOverrideResponse.error) {
			// dont continue because... idk
			console.log("ERROR AT EXISTING PRICES:", existingPricesToOverrideResponse);
			return;
		}

		const existingPrices = existingPricesToOverrideResponse.data;
		const finalPricesDict = createDictFromArray(existingPrices, (obj) => obj.auction_items!.name);

		const finalDataDict: Record<string, Database["public"]["Tables"]["auction_prices"]["Update"]> = {};

		for (const key in finalPricesDict) {
			const { auction_items, average_price, created_at, id, item_id, total_sold } = finalPricesDict[key];
			finalDataDict[auction_items!.name] = { average_price, created_at, item_id, total_sold };
		}

		for (const item of this.parsedData) {
			if (!finalDataDict[item.name]) {
				finalDataDict[item.name] = {
					average_price: item.price,
					created_at: item.created_at,
					item_id: allItemsDict[item.name].id,
					total_sold: 1,
				};
			} else {
				const pV = finalDataDict[item.name];
				const { average_price, created_at, id, item_id, total_sold } = pV;
				const { bin, category, name, price, tier } = item;
				finalDataDict[item.name].average_price = Math.round(
					(average_price! * total_sold! + item.price) / (total_sold! + 1)
				);
				finalDataDict[item.name].total_sold!++;
			}
		}

		const finalDataArray: Database["public"]["Tables"]["auction_prices"]["Insert"][] = [];

		for (const key in finalDataDict) {
			const { average_price, created_at, id, item_id, total_sold } = finalDataDict[key];
			finalDataArray.push({
				average_price: average_price!,
				created_at: created_at!,
				item_id: item_id!,
				total_sold: total_sold!,
			});
		}

		const overriddenSuccessResponse = await supabaseClient.client.from("auction_prices").upsert(finalDataArray);
	}
}
