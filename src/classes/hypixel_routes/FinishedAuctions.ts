import { DB } from "../../types";
import { supabaseClient } from "../../supabase/client";
import { FinishedAuctionItem } from "./FinishedAuctionItem";
import { myUtils } from "../../utils";

/** /auctions_ended */
export class FinishedAuctions {
	success: boolean;
	lastUpdated: number;
	auctions: FinishedAuctionItem[];
	parsedData: DB.InsertItem[] = [];
	constructor(params: { success: boolean; lastUpdated: number; auctions: FinishedAuctionItem[] }) {
		this.success = params.success;
		this.lastUpdated = params.lastUpdated;
		this.auctions = params.auctions;
	}

	/** handles adding only new items to the database, as well as adding new prices for each found item */
	public async SaveDataToDatabase() {
		// 1. fetch item rows where items match of the same type
		// 2. update existing item rows
		// 2. add new item rows where they dont already exist

		const stack = 20;
		const pricesData: DB.RowItem[] = [];

		const [names, tiers, lores] = this.parsedData.reduce(
			(pV, cV) => {
				pV[0].push(myUtils.RemoveSpecialText(cV.name));
				pV[1].push(myUtils.RemoveSpecialText(cV.tier));
				pV[2].push(myUtils.RemoveSpecialText(cV.lore));
				return pV;
			},
			[[], [], []] as string[][],
		);

		// splitting it to 500 requests cuz supabase cant handle so many at once
		for (let i = 0; i < 500; i++) {
			const _names = names.slice(stack * i, (i + 1) * stack);
			const _tiers = tiers.slice(stack * i, (i + 1) * stack);
			const _lores = lores.slice(stack * i, (i + 1) * stack);

			const pricesResponse = await supabaseClient.client
				.from("auction_items")
				.select("*")
				.in("name", _names)
				.in("tier", _tiers)
				.in("lore", _lores);

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

		// creating dicts to optimize out n^2 operations
		// which gets to trillions easily in our case
		const updateItems: Record<string, DB.UpdateItem> = {};
		const addItems: Record<string, DB.InsertItem> = {};

		for (const item of pricesData) {
			updateItems[myUtils.DictName(item)] = item;
		}

		for (const item of this.parsedData) {
			const { average_price, lore, name, tier, total_sold } = item;
			if (updateItems[myUtils.DictName(item)]) {
				updateItems[myUtils.DictName(item)].average_price = Math.round(
					(item.average_price! * total_sold! + average_price) / (total_sold! + 1),
				);
				updateItems[myUtils.DictName(item)].total_sold!++;
				continue;
			}

			if (addItems[myUtils.DictName(item)]) {
				addItems[myUtils.DictName(item)].average_price = Math.round(
					(item.average_price! * total_sold! + average_price) / (total_sold! + 1),
				);
				addItems[myUtils.DictName(item)].total_sold!++;
				continue;
			}

			addItems[myUtils.DictName(item)] = item;
		}

		let failed = false;

		for (const key in addItems) {
			const item = addItems[key];
			const _addItems = await supabaseClient.client.from("auction_items").insert(item).select();

			if (_addItems.error) {
				console.log("ERROR AT ADDING ITEMS:", _addItems);
				failed = true;
			}
		}

		for (const key in updateItems) {
			const { average_price, id, lore, name, tier, total_sold } = updateItems[key];

			const _updateItems = await supabaseClient.client
				.from("auction_items")
				.update({ average_price, total_sold })
				.eq("id", id!);

			if (_updateItems.error) {
				console.log("ERROR AT UPDATING ITEMS:", _updateItems);
				failed = true;
			}
		}

		if (!failed) {
			console.log("saved to db with no issues");
		}
	}
}
