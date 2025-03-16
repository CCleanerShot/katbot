import type { ItemType } from '$lib/types';
import { type BazaarItemsAll } from '$lib/mongodb/collections/BazaarItemsAll';
import type { AuctionItemsAll } from '$lib/mongodb/collections/AuctionItemsAll';
import type { AuctionTags } from '$lib/mongodb/collections/AuctionTags';

type CacheType = Record<string, any> & Record<ItemType, any>;
//** refers to all available items from the auction and the bazaar  */
export const cacheState = $state({
	AUCTIONS: { items: [] as AuctionItemsAll[], tags: [] as AuctionTags[] },
	BAZAAR: [] as BazaarItemsAll[]
} satisfies CacheType);
