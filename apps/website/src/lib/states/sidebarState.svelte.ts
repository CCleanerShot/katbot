import type { AuctionSocketMessage } from '$lib/types';
import type { BazaarBuy } from '$lib/mongodb/collections/BazaarBuy';
import type { BazaarSell } from '$lib/mongodb/collections/BazaarSell';

export const sidebarState = $state({
	SkyblockAlertsSidebar: {
		isOpened: false as boolean,
		items: { auctionSocketMessages: [] as AuctionSocketMessage[], bazaarBuys: [] as BazaarBuy[], bazaarSells: [] as BazaarSell[] }
	}
});
