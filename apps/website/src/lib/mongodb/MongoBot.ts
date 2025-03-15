import {
	SVELTE_MONGODB_BASE_URI,
	SVELTE_MONGODB_C_AUCTION_BUY,
	SVELTE_MONGODB_C_AUCTION_ITEMS,
	SVELTE_MONGODB_C_BAZAAR_BUY,
	SVELTE_MONGODB_C_BAZAAR_ITEMS,
	SVELTE_MONGODB_C_BAZAAR_SELL,
	SVELTE_MONGODB_C_ROLL_STATS,
	SVELTE_MONGODB_C_SESSIONS,
	SVELTE_MONGODB_C_STARBOARDS,
	SVELTE_MONGODB_C_USERS,
	SVELTE_MONGODB_D_DISCORD,
	SVELTE_MONGODB_D_GENERAL,
	SVELTE_MONGODB_D_HYPIXEL,
	SVELTE_MONGODB_OPTIONS
} from '$env/static/private';
import type { BazaarItem } from './BazaarItem';
import { Db, MongoClient } from 'mongodb';
import type { BazaarItemsAll } from './collections/BazaarItemsAll';
import { MongoCollection } from './MongoCollection';
import type { Session, User } from '$lib/types';

export class MongoBot {
	Client: MongoClient;
	private MONGODB_D_DISCORD: Db;
	private MONGODB_D_GENERAL: Db;
	private MONGODB_D_HYPIXEL: Db;

	public MONGODB_C_AUCTION_BUY: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_C_AUCTION_ITEMS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_C_BAZAAR_BUY: MongoCollection<BazaarItem>;
	public MONGODB_C_BAZAAR_ITEMS: MongoCollection<BazaarItemsAll>;
	public MONGODB_C_BAZAAR_SELL: MongoCollection<BazaarItem>;
	public MONGODB_C_SESSIONS: MongoCollection<Session>;
	public MONGODB_C_STARBOARDS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_C_USERS: MongoCollection<User>;
	public MONGODB_C_ROLL_STATS: MongoCollection<BazaarItem>; // TODO: implement

	constructor() {
		this.Client = new MongoClient(SVELTE_MONGODB_BASE_URI + SVELTE_MONGODB_OPTIONS);
		this.MONGODB_D_DISCORD = this.Client.db(SVELTE_MONGODB_D_DISCORD);
		this.MONGODB_D_GENERAL = this.Client.db(SVELTE_MONGODB_D_GENERAL);
		this.MONGODB_D_HYPIXEL = this.Client.db(SVELTE_MONGODB_D_HYPIXEL);

		this.MONGODB_C_AUCTION_BUY = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(SVELTE_MONGODB_C_AUCTION_BUY));
		this.MONGODB_C_AUCTION_ITEMS = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(SVELTE_MONGODB_C_AUCTION_ITEMS));
		this.MONGODB_C_BAZAAR_BUY = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(SVELTE_MONGODB_C_BAZAAR_BUY));
		this.MONGODB_C_BAZAAR_ITEMS = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(SVELTE_MONGODB_C_BAZAAR_ITEMS));
		this.MONGODB_C_BAZAAR_SELL = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(SVELTE_MONGODB_C_BAZAAR_SELL));
		this.MONGODB_C_SESSIONS = new MongoCollection(this.MONGODB_D_GENERAL.collection(SVELTE_MONGODB_C_SESSIONS));
		this.MONGODB_C_STARBOARDS = new MongoCollection(this.MONGODB_D_DISCORD.collection(SVELTE_MONGODB_C_STARBOARDS));
		this.MONGODB_C_USERS = new MongoCollection(this.MONGODB_D_GENERAL.collection(SVELTE_MONGODB_C_USERS));
		this.MONGODB_C_ROLL_STATS = new MongoCollection(this.MONGODB_D_DISCORD.collection(SVELTE_MONGODB_C_ROLL_STATS));
	}
}
