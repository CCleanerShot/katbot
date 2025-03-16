import type { BazaarType } from '$lib/types';
import type { BazaarBuy } from '$lib/mongodb/collections/BazaarBuy';
import type { BazaarSell } from '$lib/mongodb/collections/BazaarSell';

/** refers to the user's own bazaar items */
export const bazaarState = $state({ BUYS: [] as BazaarBuy[], SELLS: [] as BazaarSell[] } satisfies Record<BazaarType, object>);
