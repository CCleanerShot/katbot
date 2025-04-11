import type { AuctionTag } from './mongodb/AuctionTag';
import type { BazaarItem } from './mongodb/BazaarItem';
import type { AuctionBuy } from './mongodb/collections/AuctionBuy';

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
	RequestedItem: AuctionBuy;
};

export type BazaarRouteProduct = {
	product_id: string;
	/** The list of the top buy orders. */
	buy_summary: BazaarRouteSummaryItem[];
	/** The list of the top sell orders. */
	sell_summary: BazaarRouteSummaryItem[];
	quick_status: BazaarRouteQuickStatus;
};

export type BazaarRouteSummaryItem = {
	amount: bigint;
	pricePerUnit: number;
	orders: number;
};

export type BazaarRouteQuickStatus = {
	productId: string;
	buyOrders: number;
	buyPrice: number;
	buyMovingWeek: bigint;
	buyVolume: bigint;
	sellOrders: number;
	sellPrice: number;
	sellMovingWeek: bigint;
	sellVolume: bigint;
};

export type BazaarSocketMessage = {
	LiveSummary: BazaarRouteProduct;
	RequestedItem: BazaarItem;
};

export enum SocketMessageType {
	AUCTIONS,
	BAZAAR
}

export type SocketMessage = {
	id: number;
	type: SocketMessageType;
	auctionSocketMessages: AuctionSocketMessage[];
	bazaarSocketMessagesBuy: BazaarSocketMessage[];
	bazaarSocketMessagesSell: BazaarSocketMessage[];
};
