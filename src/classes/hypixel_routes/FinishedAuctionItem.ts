export class FinishedAuctionItem {
    auction_id: string // "a21f2ced497c456193f9b3d7be2adf61";
    seller: string // "79d32d29ad224d2286559f7141db6d31";
    seller_profile: string // "64b6177e09ad4a47aa4df95954d1a451";
    buyer: string // "ee309ca8378548018fe3d093cf387dfa";
    buyer_profile: string // "728dc90b7cd0442c9ca7d12c4c07ef2f";
    timestamp: number // 1731767591394;
    price: number // 2000000;
    bin: boolean // true;
    item_bytes: string // "string|Buffer";

    constructor(params: {
        auction_id: string;
        seller: string;
        seller_profile: string;
        buyer: string;
        buyer_profile: string;
        timestamp: number;
        price: number;
        bin: boolean;
        item_bytes: string;
    }) {
        this.auction_id = params.auction_id;
        this.seller = params.seller;
        this.seller_profile = params.seller_profile;
        this.buyer = params.buyer;
        this.buyer_profile = params.buyer_profile;
        this.timestamp = params.timestamp;
        this.price = params.price;
        this.bin = params.bin;
        this.item_bytes = params.item_bytes;
    }

    public SaveData(): void {

    }
}