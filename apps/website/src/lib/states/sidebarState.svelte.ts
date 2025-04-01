import type { AuctionSocketMessage } from '$lib/types';
import type { BazaarBuy } from '$lib/mongodb/collections/BazaarBuy';
import type { BazaarSell } from '$lib/mongodb/collections/BazaarSell';

export const sidebarState = $state({
	SkyblockAlertsSidebar: {
		isOpened: true as boolean,
		items: { auctionItems: [] as AuctionSocketMessage[], bazaarBuys: [] as BazaarBuy[], bazaarSells: [] as BazaarSell[] }
	}
});
