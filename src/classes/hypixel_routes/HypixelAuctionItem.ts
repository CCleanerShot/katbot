/** all items are implied to be BIN */
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
		this.name = params.name;
		this.created_at = params.created_at;
		this.category = params.category;
		this.price = params.price;
		this.tier = params.tier;
	}
}
