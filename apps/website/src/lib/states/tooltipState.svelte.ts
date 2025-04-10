import type { BazaarItem } from '$lib/mongodb/BazaarItem';
import type { AuctionBuy } from '../mongodb/collections/AuctionBuy';
import type { AuctionsRouteProductMinimal, BazaarRouteProduct } from '$lib/types';

export const tooltipState = $state({
	AuctionProductInfoTooltip: {
		buy: {} as AuctionBuy,
		product: {} as AuctionsRouteProductMinimal
	},
	BazaarProductInfoTooltip: {
		item: {} as BazaarItem,
		product: {} as BazaarRouteProduct,
		type: 'BUY' as 'BUY' | 'SELL'
	}
});

export const tooltipConfigState = $state({ closeButton: 'ESC' });
export const tooltipIsOpened = $state({ current: '' as keyof typeof tooltipState | 'NONE' });
