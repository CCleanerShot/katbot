import { settings } from "../Settings";

class FetchBot {
    readonly headers: HeadersInit = { "API-Key": settings.HYPIXEL_BOT_KEY };
    constructor() {}

    async FetchItems() {
        const response = await fetch(settings.HYPIXEL_API_BASE_URL, { headers: this.headers });
    }
}

export const fetchbot = new FetchBot();
