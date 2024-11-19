import { utils } from "../../utils";
import { FinishedAuctionItem } from "./FinishedAuctionItem";
import nbt from "prismarine-nbt"
import { HypixelAuctionItem } from "./HypixelAuctionItem";
import { supabaseClient } from "../../supabase/client";
import { HypixelAuctionItemWithID } from "./HypixelAuctionItemWithID";

/** /auctions_ended */
export class FinishedAuctions {
    success: boolean;
    lastUpdated: number;
    auctions: FinishedAuctionItem[]
    parsedData: HypixelAuctionItem[] = [];
    constructor(params: {
        success: boolean,
        lastUpdated: number,
        auctions: FinishedAuctionItem[],
    }) {
        this.success = params.success;
        this.lastUpdated = params.lastUpdated;
        this.auctions = params.auctions;
    }

    /** parses the raw data from the /auctions route, and sets the parsedData to the selected data from the Buffer (converted to NBT) */
    public async ParseRawData(): Promise<void> {
        const results: HypixelAuctionItem[] = [];

        for (let i = 0; i < this.auctions.length; i++) {
            const auction = this.auctions[i];
            const data = Buffer.from(auction.item_bytes, "base64");
            const nestedData = ((await nbt.parse(data)).parsed.value as any)!.i!.value!.value![0].tag.value; // ya.....

            const price: number = auction.price;

            // either its an outlier sell, or the item is so worthless it doesnt matter
            if (price < 100000) {
                continue;
            }

            const bin: boolean = auction.bin;
            const name: string = utils.RemoveSpecialText(nestedData.display.value.Name.value);
            const lore: string = nestedData.display.value.Lore.value.value;

            // the last item in the array contains the rarity and item type
            let lastLine = utils.RemoveSpecialText(lore[lore.length - 1]).trim();

            //// cut off from the first space
            const cutIndex = lastLine.indexOf(" ");

            let rarity: string = "";
            let category: string = "";

            if (cutIndex == -1) {
                rarity = lastLine;
            } else {
                category = lastLine.slice(cutIndex);
                rarity = lastLine.slice(0, cutIndex + 1);
            }

            // results.push(new HypixelAuctionItem({ bin, name, category, price, tier: rarity, }))
        }

        this.parsedData = results;
    }

    /** handles adding only new items to the database, as well as adding new prices for each found item */
    public async SaveDataToDatabase() {
        const currentItems = await supabaseClient.client.from("auction_items").select("*");
        const toAdd = this.parsedData.filter(d => currentItems.data?.find(item => item.name == d.name && item.tier == d.tier && item.category == d.category))

        await supabaseClient.client.from("auction_items").insert(toAdd);

        // const withID: HypixelAuctionItemWithID[] = toAdd.map(d => new HypixelAuctionItemWithID(d.bin));
        // await supabaseClient.client.from("auction_prices").insert(withID)
    }
}