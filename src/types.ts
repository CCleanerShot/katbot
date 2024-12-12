export namespace DB {
	export type BazaarItem = {
		name: string;
	};

	export type AuctionItem = {
		name: string;
		tier: string;
		lore: string;
		average_price: number;
		total_sold: number;
		created_at: number;
	};
}
