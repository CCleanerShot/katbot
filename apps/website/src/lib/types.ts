import type { cacheState } from './states/cacheState.svelte';

export type ArrayType = { Name: string; beg: string; mid: string; end: string }[];
export type BasePageData = { description: string; title: string };
export type BazaarType = 'BUYS' | 'SELLS';
export type ItemType = 'AUCTIONS' | 'BAZAAR';
export type ToastProps = { id: number; message: string; type: 'WARNING' | 'ERROR' | 'NONE' };
