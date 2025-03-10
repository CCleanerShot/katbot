import type { ItemType } from '$lib/types';
import { type BazaarItemsAll } from '$lib/mongodb/BazaarItemsAll/interface';

type CacheType = Record<string, any> & Record<ItemType, any[]>
//** refers to all available items from the auction and the bazaar  */
export const cacheState = $state({ AUCTIONS: [], BAZAAR: [] as BazaarItemsAll[] } satisfies CacheType);
