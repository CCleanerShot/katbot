import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = () => {
    return {
        title: "Skyblock",
        description: "Edit your Hypixel Skyblock details! Currently contains features for the Auction and Bazaar."
    }
}