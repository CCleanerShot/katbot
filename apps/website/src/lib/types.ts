import type { AuctionTag } from './mongodb/AuctionTag';
import type { AuctionBuy } from './mongodb/collections/AuctionBuy';
import type { BazaarBuy } from './mongodb/collections/BazaarBuy';
import type { BazaarSell } from './mongodb/collections/BazaarSell';

export type ArrayType = { Name: string; beg: string; mid: string; end: string }[];
export type BasePageData = { description: string; title: string };
export type BazaarType = 'BUYS' | 'SELLS';
export type ItemType = 'AUCTIONS' | 'BAZAAR';
export type ToastProps = { id: number; message: string; type: 'WARNING' | 'ERROR' | 'NONE' };

/** needed for using the data from the auction itself (via websocket)*/
export type AuctionsRouteProductMinimal = {
	uuid: string;
	auctioneer: string;
	starting_bid: bigint;
	highest_bid_amount: bigint;

	ITEM_ID: string;
	ITEM_NAME: string;
	AuctionTags: AuctionTag[];
};

export type AuctionSocketMessage = {
	LiveItems: AuctionsRouteProductMinimal[];
	BuyItem: AuctionBuy;
};

export enum SocketMessageType {
	AUCTIONS,
	BAZAAR
}

export type SocketMessage = {
	id: number;
	type: SocketMessageType;
	auctionSocketMessages: AuctionSocketMessage[];
	bazaarBuys: BazaarBuy[];
	bazaarSells: BazaarSell[];
};
