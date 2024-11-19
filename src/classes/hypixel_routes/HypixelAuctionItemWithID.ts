import { HypixelAuctionItem } from "./HypixelAuctionItem";

/** all items are implied to be BIN. this has an ID, and is implied to be existing from the database */
export class HypixelAuctionItemWithID extends HypixelAuctionItem {
    item_id: string // uuid

    constructor(params: { item_id: string; bin: boolean; name: string, category: string, date_sold: string, price: number, tier: string, }) {
        super(params);
        this.item_id = params.item_id;
    }
}