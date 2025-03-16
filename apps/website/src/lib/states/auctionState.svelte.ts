import type { AuctionBuy as AuctionItem } from '$lib/mongodb/collections/AuctionBuy';

/** refers to the user's own auction items */
export const auctionState = $state({ BUYS: [] as AuctionItem[], ITEM: {} as AuctionItem } satisfies Record<'BUYS' | string, object>);
