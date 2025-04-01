import processENV from "dotenv";

class Settings {
    HYPIXEL_API_BASE_URL: string;
    HYPIXEL_BOT_KEY: string;
    MONGODB_BASE_URI: string;
    MONGODB_C_AUCTION_BUY: string;
    MONGODB_C_AUCTION_ITEMS: string;
    MONGODB_C_AUCTION_TAGS: string;
    MONGODB_C_BAZAAR_BUY: string;
    MONGODB_C_BAZAAR_ITEMS: string;
    MONGODB_C_BAZAAR_SELL: string;
    MONGODB_C_ROLL_STATS: string;
    MONGODB_C_SESSIONS: string;
    MONGODB_C_STARBOARDS: string;
    MONGODB_C_USERS: string;
    MONGODB_D_DISCORD: string;
    MONGODB_D_GENERAL: string;
    MONGODB_D_HYPIXEL: string;
    MONGODB_OPTIONS: string;

    constructor() {
        processENV.config();

        this.HYPIXEL_API_BASE_URL = process.env.HYPIXEL_API_BASE_URL!;
        this.HYPIXEL_BOT_KEY = process.env.HYPIXEL_BOT_KEY!;
        this.MONGODB_BASE_URI = process.env.MONGODB_BASE_URI!;
        this.MONGODB_C_AUCTION_BUY = process.env.MONGODB_C_AUCTION_BUY!;
        this.MONGODB_C_AUCTION_ITEMS = process.env.MONGODB_C_AUCTION_ITEMS!;
        this.MONGODB_C_AUCTION_TAGS = process.env.MONGODB_C_AUCTION_TAGS!;
        this.MONGODB_C_BAZAAR_BUY = process.env.MONGODB_C_BAZAAR_BUY!;
        this.MONGODB_C_BAZAAR_ITEMS = process.env.MONGODB_C_BAZAAR_ITEMS!;
        this.MONGODB_C_BAZAAR_SELL = process.env.MONGODB_C_BAZAAR_SELL!;
        this.MONGODB_C_ROLL_STATS = process.env.MONGODB_C_ROLL_STATS!;
        this.MONGODB_C_SESSIONS = process.env.MONGODB_C_SESSIONS!;
        this.MONGODB_C_STARBOARDS = process.env.MONGODB_C_STARBOARDS!;
        this.MONGODB_C_USERS = process.env.MONGODB_C_USERS!;
        this.MONGODB_D_DISCORD = process.env.MONGODB_D_DISCORD!;
        this.MONGODB_D_GENERAL = process.env.MONGODB_D_GENERAL!;
        this.MONGODB_D_HYPIXEL = process.env.MONGODB_D_HYPIXEL!;
        this.MONGODB_OPTIONS = process.env.MONGODB_OPTIONS!;
    }
}

export const settings = new Settings();
