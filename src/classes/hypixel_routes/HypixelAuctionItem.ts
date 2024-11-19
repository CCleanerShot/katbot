/** all items are implied to be BIN */
export class HypixelAuctionItem {
    bin: boolean;
    name: string;
    category: string;
    date_sold: string; // timestamp
    price: number;
    tier: string;

    constructor(params: { bin: boolean; name: string, category: string, date_sold: string, price: number, tier: string, }) {
        this.bin = params.bin;
        this.name = params.name;
        this.date_sold = params.date_sold;
        this.category = params.category;
        this.price = params.price;
        this.tier = params.tier;
    }
}