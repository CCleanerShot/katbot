export class OngoingAuctionItem {
    uuid: string // "409a1e0f261a49849493278d6cd9305a";
    auctioneer: string // "347ef6c1daac45ed9d1fa02818cf0fb6";
    profile_id: string // "347ef6c1daac45ed9d1fa02818cf0fb6";
    coop: any[] // [];
    start: number // 1573760802637;
    end: number // 1573761102637;
    item_name: string // "Azure Bluet";
    item_lore: string // "§f§lCOMMON";
    extra: string // "Azure Bluet Red Rose";
    category: string // "blocks";
    tier: string // "COMMON"
    starting_bid: number // 1;
    item_bytes: any // Buffer|string
    claimed: boolean // true;
    claimed_bidders: any[] // TODO: complete
    highest_bid_amount: number // 7607533;
    bids: any[] // TODO: complete

    constructor(params: {
        uuid: string
        auctioneer: string
        profile_id: string
        coop: any
        start: number
        end: number
        item_name: string
        item_lore: string
        extra: string
        category: string
        tier: string
        starting_bid: number
        item_bytes: any
        claimed: boolean
        claimed_bidders: any
        highest_bid_amount: number
        bids: any
    }) {
        this.uuid = params.uuid
        this.auctioneer = params.auctioneer
        this.profile_id = params.profile_id
        this.coop = params.coop
        this.start = params.start
        this.end = params.end
        this.item_name = params.item_name
        this.item_lore = params.item_lore
        this.extra = params.extra
        this.category = params.category
        this.tier = params.tier
        this.starting_bid = params.starting_bid
        this.item_bytes = params.item_bytes
        this.claimed = params.claimed
        this.claimed_bidders = params.claimed_bidders
        this.highest_bid_amount = params.highest_bid_amount
        this.bids = params.bids
    }
}