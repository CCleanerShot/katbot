import { DB } from "../../types";
import { supabaseClient } from "../../supabase/client";
import { HypixelAuctionItem } from "./HypixelAuctionItem";
import { FinishedAuctionItem } from "./FinishedAuctionItem";

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

		function createArrayFromDict<T, K extends Record<string, T>>(obj: K): T[] {
			const result: T[] = [];

			for (const key in obj) {
				const item = obj[key];
				result.push(item);
			}

			return result;
		}

		function dictName<T extends { name: string; tier: string }>(obj: T) {
			return obj.name + obj.tier;
		}

		// STEPS:
		// 1. fetch item rows where items match of the same type
		// 2. add new item rows where they dont already exist
		// 3. override price rows where the item + hour exist
		// 4. add new price rows where the item + hour dont exist

		/////////////////////
		// NEW ITEMS START //
		/////////////////////

		const _currentItems = await supabaseClient.client
			.from("auction_items")
			.select("*")
			.in(
				"name",
				this.parsedData.map((e) => e.name),
			)
			.in(
				"category",
				this.parsedData.map((e) => e.category),
			)
			.in(
				"tier",
				this.parsedData.map((e) => e.tier),
			);

		if (_currentItems.error) {
			// dont continue to prevent duplicate items
			console.log("ERROR AT CURRENT ITEMS:", _currentItems);
			return;
		}

		// creating dicts to optimize out n^2 operations
		// which gets to trillions easily in our case
		const allItemsDict: Record<string, DB.RowItem> = {};
		const itemsToAddDict = createDictFromArray(this.parsedData, (obj) => dictName(obj));

		for (const existingItem of _currentItems.data) {
			allItemsDict[dictName(existingItem)] = existingItem;
			const potentialItem = { ...itemsToAddDict[dictName(existingItem)] };

			if (potentialItem && potentialItem.tier === existingItem.tier) {
				delete itemsToAddDict[dictName(existingItem)];
			}
		}

		const itemsToAddArray: DB.InsertItem[] = [];

		for (const key in itemsToAddDict) {
			const { category, created_at, name, tier } = itemsToAddDict[key];
			itemsToAddArray.push({ category, created_at, name, tier });
		}

		const _addItems = await supabaseClient.client.from("auction_items").insert(itemsToAddArray).select();

		if (_addItems.error) {
			// dont continue to prevent missing items when adding prices
			console.log("ERROR AT ADDING ITEMS:", _addItems);
			return;
		}

		for (const item of _addItems.data) {
			allItemsDict[dictName(item)] = item;
		}

		///////////////////
		// NEW ITEMS END //
		///////////////////

		//////////////////
		// PRICES START //
		//////////////////
		const _existingPricesToOverride = await supabaseClient.client
			.from("auction_prices")
			.select(
				`
				*,
				auction_items ( * )
			`,
			)
			.in("item_id", [..._currentItems.data.map((e) => e.id), ..._addItems.data.map((e) => e.id)]);

		if (_existingPricesToOverride.error) {
			console.log("ERROR AT EXISTING PRICES:", _existingPricesToOverride);
			return;
		}

		const ap: Record<string, DB.InsertPrice> = {};
		// biome-ignore format: fuck off
		const op = _existingPricesToOverride.data.reduce((pV, cV) => {
			const {auction_items, average_price, created_at, id, item_id, total_sold} = cV
			pV[dictName(auction_items!)] = {average_price, created_at, id, item_id, total_sold}
			return pV;
		}, {} as Record<string, DB.UpdatePrice>)

		for (const item of this.parsedData) {
			const { bin, category, created_at, name, price, tier } = item;
			switch (true as boolean) {
				case !!op[dictName(item)]: {
					const { average_price, created_at, id, item_id, total_sold } = op[dictName(item)];
					op[dictName(item)].average_price = Math.round((average_price! * total_sold! + price) / (total_sold! + 1));
					op[dictName(item)].total_sold!++;
					break;
				}
				case !!ap[dictName(item)]: {
					const { average_price, created_at, id, item_id, total_sold } = ap[dictName(item)];
					ap[dictName(item)].average_price = Math.round((average_price! * total_sold! + price) / (total_sold! + 1));
					ap[dictName(item)].total_sold!++;
					break;
				}
				default: {
					ap[dictName(item)] = {
						average_price: price,
						created_at: created_at,
						item_id: allItemsDict[dictName(item)].id,
						total_sold: 1,
					};
					break;
				}
			}
		}

		const overrideD: DB.UpdatePrice[] = createArrayFromDict(op);
		const addD: DB.InsertPrice[] = createArrayFromDict(ap);

		const _addSuccess = await supabaseClient.client.from("auction_prices").insert(addD);

		if (_addSuccess.error) {
			console.log("ERROR AT ADDING PRICES:", _addSuccess);
			return;
		}

		let failed = false;
		// why cant i mass update
		for (const item of overrideD) {
			const _overriddenSuccess = await supabaseClient.client.from("auction_prices").update(item).eq("id", item.id!);

			if (_overriddenSuccess.error) {
				console.log("ERROR AT OVERRIDING PRICES:", _overriddenSuccess);
				failed = true;
			}
		}

		////////////////
		// PRICES END //
		////////////////

		if (!failed) {
			console.log("got more data!", Date.now());
		}
	}
}
