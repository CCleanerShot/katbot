import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = () => {
    return {
        title: "Home",
        description: "Home of the KatBot."
    }
}