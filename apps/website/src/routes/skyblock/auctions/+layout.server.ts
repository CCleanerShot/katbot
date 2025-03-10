import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = () => {
    return {
        title: "Auctions",
        description: "Edit your Hypixel Skyblock auction watchlist! Features tracking enchantments, attributes, reforges, and more!"
    }
}