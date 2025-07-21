import type { AuctionTag } from './mongodb/AuctionTag';
import type { BazaarItem } from './mongodb/BazaarItem';
import type { AuctionBuy } from './mongodb/collections/AuctionBuy';

export type ArrayType = { Name: string; beg: string; mid: string; end: string }[];
export type BasePageData = { description: string; title: string };
export type BazaarType = 'BUYS' | 'SELLS';
export type ItemType = 'AUCTIONS' | 'BAZAAR';
export type Provider = 'email' | 'discord';
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

// https://datatracker.ietf.org/doc/html/rfc6749
export type OAuthAuthorizeQuery = {
	client_id: string;
	redirect_uri: string;
	response_type: 'code';
	scope: string;
};

export type OAuthTokenHeaders = {
	'Accept-Encoding': 'application/x-www-form-urlencoded';
	'Content-Type': 'application/x-www-form-urlencoded';
};

export type OAuthTokenParams = {
	client_id: string;
	client_secret: string;
	code: string;
	grant_type: 'authorization_code';
	redirect_uri: string;
};

export type OAuthTokenResponse = {
	token_type: 'Bearer';
	access_token: string;
	expires_in: number;
	refresh_token: string;
	scope: string;
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
