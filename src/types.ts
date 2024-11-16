// hypixel provides a schematic at https://api.hypixel.net/, but would take some time to implement
// for now, we are hardcoding the requests

export namespace RequestTypes {
	export type Auctions = typeof RequestConst.auctions;
	export type AuctionItem = typeof RequestConst.auctionItem;
}

export namespace RequestConst {
	export const auctions = {
		success: true as boolean,
		page: 0 as number,
		totalPages: 32 as number,
		totalAuctions: 31267 as number,
		lastUpdated: 1571065561345 as number,
		auctions: [] as RequestTypes.AuctionItem[],
	} as const;
	export const auctionItem = {
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
	};
}
