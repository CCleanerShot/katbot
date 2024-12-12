import { Collection, MongoClient, MongoClientOptions } from "mongodb";
import { BotEnvironment } from "../environment";
import { DB } from "../types";

class CustomMongoDB extends MongoClient {
	auctionCollection: Collection<DB.AuctionItem>;
	bazaarCollection: Collection<DB.BazaarItem>;

	constructor(url: string, options: MongoClientOptions) {
		super(url, options);

		this.on("connectionCreated", () => {
			console.log("mongoDB connected!");
		});

		this.db();
		const db = this.db(BotEnvironment.MONGODB_DATABASE_NAME);
		this.auctionCollection = db.collection<DB.AuctionItem>(BotEnvironment.MONGODB_COLLECTION_AUCTION_ITEMS_NAME);
		this.bazaarCollection = db.collection<DB.BazaarItem>(BotEnvironment.MONGODB_COLLECTION_BAZAAR_ITEMS_NAME);
	}
}

const url = `mongodb+srv://dU6D99w1:${BotEnvironment.MONGODB_PASSWORD}@skyblock-data.lk4zj.mongodb.net/?retryWrites=true&w=majority&appName=skyblock-data`;
export const mongoClient = new CustomMongoDB(url, {});
