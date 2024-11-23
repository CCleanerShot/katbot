export class HypixelAuctionItem {
	bin: boolean;
	name: string;
	category: string;
	created_at: number; // timestamp
	price: number;
	tier: string;

	constructor(params: {
		bin: boolean;
		name: string;
		category: string;
		created_at: number;
		price: number;
		tier: string;
	}) {
		this.bin = params.bin;
		this.name = params.name.trim();
		this.created_at = params.created_at;
		this.category = params.category.trim();
		this.price = params.price;
		this.tier = params.tier.trim();
	}
}
