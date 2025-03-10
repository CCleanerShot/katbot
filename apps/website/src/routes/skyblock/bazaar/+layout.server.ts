import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = () => {
    return {
        title: "Bazaar",
        description: "Edit your Hypixel Skyblock bazaar wishlist! Features tracking buy orders, sell orders, etc."
    }
}