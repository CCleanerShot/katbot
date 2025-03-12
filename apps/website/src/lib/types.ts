import type { cacheState } from "./states/cacheState.svelte";

export type ArrayType = ((typeof cacheState)[keyof typeof cacheState][number] & { beg: string, mid: string, end: string })[];
export type BasePageData = { description: string, title: string }
export type BazaarType = 'BUYS' | 'SELLS';
export type ItemType = 'AUCTIONS' | 'BAZAAR';
