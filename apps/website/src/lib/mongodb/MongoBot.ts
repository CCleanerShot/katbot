import {
	MONGODB_BASE_URI,
	MONGODB_BASE_URI_TEST,
	MONGODB_C_AUCTION_BUY,
	MONGODB_C_AUCTION_ITEMS,
	MONGODB_C_AUCTION_TAGS,
	MONGODB_C_AUTH_DISCORD,
	MONGODB_C_AUTH_GOOGLE,
	MONGODB_C_AUTH_USERS,
	MONGODB_C_BAZAAR_BUY,
	MONGODB_C_BAZAAR_ITEMS,
	MONGODB_C_BAZAAR_SELL,
	MONGODB_C_ROLL_STATS,
	MONGODB_C_SESSIONS,
	MONGODB_C_STARBOARDS,
	MONGODB_D_AUTH,
	MONGODB_D_DISCORD,
	MONGODB_D_GENERAL,
	MONGODB_D_HYPIXEL,
	MONGODB_OPTIONS
} from '$env/static/private';
import { Db, MongoClient } from 'mongodb';
import type { BazaarItem } from './BazaarItem';
import { MongoCollection } from './MongoCollection';
import type { Session } from './collections/Session';
import type { AuthUser } from './collections/AuthUser';
import type { AuctionBuy } from './collections/AuctionBuy';
import type { AuctionTags } from './collections/AuctionTags';
import type { BazaarItemsAll } from './collections/BazaarItemsAll';
import type { AuctionItemsAll } from './collections/AuctionItemsAll';
import { PUBLIC_ENVIRONMENT } from '$env/static/public';
import type { AuthProviderDiscord } from './collections/AuthProviderDiscord';
import type { AuthProviderGoogle } from './collections/AuthProviderGoogle';

export class MongoBot {
	Client: MongoClient;
	private MONGODB_D_AUTH: Db;
	private MONGODB_D_DISCORD: Db;
	private MONGODB_D_GENERAL: Db;
	private MONGODB_D_HYPIXEL: Db;

	public MONGODB_C_AUCTION_BUY: MongoCollection<AuctionBuy>;
	public MONGODB_C_AUCTION_ITEMS: MongoCollection<AuctionItemsAll>;
	public MONGODB_C_AUCTION_TAGS: MongoCollection<AuctionTags>;
	public MONGODB_C_AUTH_USERS: MongoCollection<AuthUser>;
	public MONGODB_C_AUTH_DISCORD: MongoCollection<AuthProviderDiscord>;
	public MONGODB_C_AUTH_GOOGLE: MongoCollection<AuthProviderGoogle>;
	public MONGODB_C_BAZAAR_BUY: MongoCollection<BazaarItem>;
	public MONGODB_C_BAZAAR_ITEMS: MongoCollection<BazaarItemsAll>;
	public MONGODB_C_BAZAAR_SELL: MongoCollection<BazaarItem>;
	public MONGODB_C_SESSIONS: MongoCollection<Session>;
	public MONGODB_C_STARBOARDS: MongoCollection<BazaarItem>; // TODO: implement
	public MONGODB_C_ROLL_STATS: MongoCollection<BazaarItem>; // TODO: implement

	constructor() {
		if (PUBLIC_ENVIRONMENT === 'development') {
			this.Client = new MongoClient(MONGODB_BASE_URI_TEST + MONGODB_OPTIONS);
		} else {
			this.Client = new MongoClient(MONGODB_BASE_URI + MONGODB_OPTIONS);
		}

		this.MONGODB_D_AUTH = this.Client.db(MONGODB_D_AUTH);
		this.MONGODB_D_DISCORD = this.Client.db(MONGODB_D_DISCORD);
		this.MONGODB_D_GENERAL = this.Client.db(MONGODB_D_GENERAL);
		this.MONGODB_D_HYPIXEL = this.Client.db(MONGODB_D_HYPIXEL);

		this.MONGODB_C_AUCTION_BUY = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_AUCTION_BUY));
		this.MONGODB_C_AUCTION_ITEMS = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_AUCTION_ITEMS));
		this.MONGODB_C_AUCTION_TAGS = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_AUCTION_TAGS));
		this.MONGODB_C_AUTH_USERS = new MongoCollection(this.MONGODB_D_AUTH.collection(MONGODB_C_AUTH_USERS));
		this.MONGODB_C_AUTH_DISCORD = new MongoCollection(this.MONGODB_D_AUTH.collection(MONGODB_C_AUTH_DISCORD));
		this.MONGODB_C_AUTH_GOOGLE = new MongoCollection(this.MONGODB_D_AUTH.collection(MONGODB_C_AUTH_GOOGLE));
		this.MONGODB_C_BAZAAR_BUY = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_BAZAAR_BUY));
		this.MONGODB_C_BAZAAR_ITEMS = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_BAZAAR_ITEMS));
		this.MONGODB_C_BAZAAR_SELL = new MongoCollection(this.MONGODB_D_HYPIXEL.collection(MONGODB_C_BAZAAR_SELL));
		this.MONGODB_C_SESSIONS = new MongoCollection(this.MONGODB_D_GENERAL.collection(MONGODB_C_SESSIONS));
		this.MONGODB_C_STARBOARDS = new MongoCollection(this.MONGODB_D_DISCORD.collection(MONGODB_C_STARBOARDS));
		this.MONGODB_C_ROLL_STATS = new MongoCollection(this.MONGODB_D_DISCORD.collection(MONGODB_C_ROLL_STATS));
	}
}
