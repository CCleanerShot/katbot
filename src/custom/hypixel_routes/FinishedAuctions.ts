import { WithId } from "mongodb";

import { DB } from "../../types";
import { myUtils } from "../../utils";
import { mongoClient } from "../CustomMongoDB";
import { FinishedAuctionItem } from "./FinishedAuctionItem";
import { mongo } from "mongoose";

/** /auctions_ended */
export class FinishedAuctions {
	success: boolean;
	lastUpdated: number;
	auctions: FinishedAuctionItem[];
	parsedData: DB.AuctionItem[];
	constructor(params: { success: boolean; lastUpdated: number; auctions: FinishedAuctionItem[] }) {
		this.success = params.success;
		this.lastUpdated = params.lastUpdated;
		this.auctions = params.auctions;
		this.parsedData = [];
	}

	/** handles adding only new items to the database, as well as adding new prices for each found item */
	public async SaveDataToDatabase() {
		// 1. fetch item rows where items match of the same type
		// 2. update existing item rows
		// 2. add new item rows where they dont already exist
		const existingData: WithId<DB.AuctionItem>[] = [];

		const cursor = await mongoClient.auctionCollection.find({
			$or: this.parsedData.map(({ average_price, created_at, lore, name, tier, total_sold }) => ({
				name,
				tier,
				lore,
			})),
		});

		for await (const data of cursor) {
			existingData.push(data);
		}

		// creating dicts to optimize out n^2 operations
		// which gets to trillions easily in our case
		const updateItems: Record<string, WithId<DB.AuctionItem>> = {};
		const addItems: Record<string, DB.AuctionItem> = {};

		for (const item of existingData) {
			updateItems[myUtils.DictName(item)] = item;
		}

		for (const item of this.parsedData) {
			const { average_price, lore, name, tier, total_sold } = item;
			const k = myUtils.DictName(item);
			if (updateItems[k]) {
				updateItems[k].average_price = Math.round((average_price! * total_sold! + average_price) / (total_sold! + 1));
				updateItems[k].total_sold!++;
				continue;
			}

			if (addItems[k]) {
				addItems[k].average_price = Math.round((average_price! * total_sold! + average_price) / (total_sold! + 1));
				addItems[k].total_sold!++;
				continue;
			}

			addItems[k] = item;
		}

		const updateData = existingData;
		const addData = Object.keys(addItems).map((d) => addItems[d]);

		// cannot bulk write on an empty array
		if (updateData.length !== 0) {
			const updateBulk = await mongoClient.auctionCollection.bulkWrite(
				updateData.map(({ _id, average_price, created_at, lore, name, tier, total_sold }) => ({
					updateOne: { filter: { _id }, update: { $set: { total_sold, average_price } } },
				})),
			);
		}

		if (addData.length !== 0) {
			const addBulk = await mongoClient.auctionCollection.bulkWrite(
				addData.map((d) => ({
					insertOne: { document: d },
				})),
			);
		}

		console.log("> saved to database");
	}
}
