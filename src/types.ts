// hypixel provides a schematic at https://api.hypixel.net/, but would take some time to implement
// for now, we are hardcoding the requests

export namespace RequestTypes {
	export type Auctions = typeof RequestConst.auctions;
	export type AuctionItem = typeof RequestConst.auctionsItem;
	export type AuctionEnded = typeof RequestConst.auctionsEnded;
	export type AuctionEndedItem = {
		auction_id: string;
		seller: string;
		seller_profile: string;
		buyer: string;
		buyer_profile: string;
		timestamp: number;
		price: number;
		bin: boolean;
		item_bytes: string;
		item_name?: string; // Dynamically extracted
		highest_bid_amount?: number; // Dynamically extracted
		starting_bid?: number; // Optional if needed
	};	
}

/**
 * the const values represent examples if u check
 */
export namespace RequestConst {
	export const auctions = {
		success: true as boolean,
		page: 0 as number,
		totalPages: 32 as number,
		totalAuctions: 31267 as number,
		lastUpdated: 1571065561345 as number,
		auctions: [] as RequestTypes.AuctionItem[],
	} as const;
	export const auctionsItem = {
		uuid: "409a1e0f261a49849493278d6cd9305a" as string,
		auctioneer: "347ef6c1daac45ed9d1fa02818cf0fb6" as string,
		profile_id: "347ef6c1daac45ed9d1fa02818cf0fb6" as string,
		coop: [] as any[],
		start: 1573760802637 as number,
		end: 1573761102637 as number,
		item_name: "Azure Bluet" as string,
		item_lore: "§f§lCOMMON" as string,
		extra: "Azure Bluet Red Rose" as string,
		category: "blocks" as string,
		tier: "COMMON" as string, // might be enum
		starting_bid: 1 as number,
		item_bytes: {} as any,
		claimed: true as boolean,
		claimed_bidders: [] as any[],
		highest_bid_amount: 7607533 as number,
		bids: [] as any[],
	} as const;
	export const auctionsEnded = {
		success: true as boolean,
		lastUpdated: 1607456463916 as number,
		auctions: [] as RequestTypes.AuctionEndedItem[],
	};
	export const auctionsEndedItem = {
		auction_id: "a21f2ced497c456193f9b3d7be2adf61",
		seller: "79d32d29ad224d2286559f7141db6d31",
		seller_profile: "64b6177e09ad4a47aa4df95954d1a451",
		buyer: "ee309ca8378548018fe3d093cf387dfa",
		buyer_profile: "728dc90b7cd0442c9ca7d12c4c07ef2f",
		timestamp: 1731767591394,
		price: 2000000,
		bin: true,
		item_bytes: "a large string",
		// Add the optional fields for consistency
		//item_name: "Azure Bluet",
		//highest_bid_amount: 7607533,
		//starting_bid: 1,
	} as const;
}
