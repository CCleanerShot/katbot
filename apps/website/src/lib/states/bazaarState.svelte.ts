import type { BazaarType } from "$lib/types";
import { type BazaarItem } from '$lib/mongodb/BazaarItem';

/** refers to the user's own bazaar items */
export const bazaarState = $state({BUYS: [] as BazaarItem[], SELLS: [] as BazaarItem[] } satisfies Record<BazaarType, object>)