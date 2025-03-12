import {
	SVELTE_MONGODB_BASE_URI,
	SVELTE_MONGODB_COLLECTION_AUCTION_BUY,
	SVELTE_MONGODB_COLLECTION_AUCTION_ITEMS,
	SVELTE_MONGODB_COLLECTION_BAZAAR_BUY,
	SVELTE_MONGODB_COLLECTION_BAZAAR_ITEMS,
	SVELTE_MONGODB_COLLECTION_BAZAAR_SELL,
	SVELTE_MONGODB_COLLECTION_ROLL_STATS,
	SVELTE_MONGODB_COLLECTION_STARBOARDS,
	SVELTE_MONGODB_COLLECTION_USERS,
	SVELTE_MONGODB_DATABASE_DISCORD,
	SVELTE_MONGODB_DATABASE_GENERAL,
	SVELTE_MONGODB_DATABASE_HYPIXEL,
	SVELTE_MONGODB_OPTIONS
} from '$env/static/private';
import type { BazaarItem } from './BazaarItem';
import { Db, MongoClient } from 'mongodb';
import type { BazaarItemsAll } from './collections/BazaarItemsAll';
import { MongoCollection } from './MongoCollection';

class MongoBot {
	Client: MongoClient;
	private MONGODB_DATABASE_DISCORD: Db;
	private MONGODB_DATABASE_GENERAL: Db;
	private MONGODB_DATABASE_HYPIXEL: Db;

	public MONGODB_COLLECTION_AUCTION_BUY: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_COLLECTION_AUCTION_ITEMS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_COLLECTION_BAZAAR_BUY: MongoCollection<BazaarItem>;
	public MONGODB_COLLECTION_BAZAAR_ITEMS: MongoCollection<BazaarItemsAll>;
	public MONGODB_COLLECTION_BAZAAR_SELL: MongoCollection<BazaarItem>;
	public MONGODB_COLLECTION_STARBOARDS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_COLLECTION_USERS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_COLLECTION_ROLL_STATS: MongoCollection<BazaarItem>; // TODO: implement

	constructor() {
		this.Client = new MongoClient(SVELTE_MONGODB_BASE_URI + SVELTE_MONGODB_OPTIONS);
		this.MONGODB_DATABASE_DISCORD = this.Client.db(SVELTE_MONGODB_DATABASE_DISCORD);
		this.MONGODB_DATABASE_GENERAL = this.Client.db(SVELTE_MONGODB_DATABASE_GENERAL);
		this.MONGODB_DATABASE_HYPIXEL = this.Client.db(SVELTE_MONGODB_DATABASE_HYPIXEL);

		this.MONGODB_COLLECTION_AUCTION_BUY = new MongoCollection(this.MONGODB_DATABASE_HYPIXEL.collection(SVELTE_MONGODB_COLLECTION_AUCTION_BUY));
		this.MONGODB_COLLECTION_AUCTION_ITEMS = new MongoCollection(this.MONGODB_DATABASE_HYPIXEL.collection(SVELTE_MONGODB_COLLECTION_AUCTION_ITEMS));
		this.MONGODB_COLLECTION_BAZAAR_BUY = new MongoCollection(this.MONGODB_DATABASE_HYPIXEL.collection(SVELTE_MONGODB_COLLECTION_BAZAAR_BUY));
		this.MONGODB_COLLECTION_BAZAAR_ITEMS = new MongoCollection(this.MONGODB_DATABASE_HYPIXEL.collection(SVELTE_MONGODB_COLLECTION_BAZAAR_ITEMS));
		this.MONGODB_COLLECTION_BAZAAR_SELL = new MongoCollection(this.MONGODB_DATABASE_HYPIXEL.collection(SVELTE_MONGODB_COLLECTION_BAZAAR_SELL));
		this.MONGODB_COLLECTION_STARBOARDS = new MongoCollection(this.MONGODB_DATABASE_DISCORD.collection(SVELTE_MONGODB_COLLECTION_STARBOARDS));
		this.MONGODB_COLLECTION_USERS = new MongoCollection(this.MONGODB_DATABASE_DISCORD.collection(SVELTE_MONGODB_COLLECTION_USERS));
		this.MONGODB_COLLECTION_ROLL_STATS = new MongoCollection(this.MONGODB_DATABASE_DISCORD.collection(SVELTE_MONGODB_COLLECTION_ROLL_STATS));

		// (FindCursor as any).prototype._toArray = async function () {
		// 	const result = await this.toArray();
		// 	console.log(result);
		// };

		// (Collection as any).prototype._find = function (filter?: Filter<BazaarItemsAll>, options?: FindOptions & Abortable) {
		// 	return this.find(filter, options);
		// };

		// const test = this.MONGODB_COLLECTION_BAZAAR_SELL._find({ UserId: 208963262094639104n }).toArray();
	}
}

export const mongoBot = new MongoBot();
