import type { AuctionsRouteProductMinimal } from '$lib/types';
import type { AuctionBuy } from '../mongodb/collections/AuctionBuy';

export const tooltipState = $state({
	AuctionProductInfoTooltip: {
		isOpened: false,
		buy: {} as AuctionBuy,
		product: {} as AuctionsRouteProductMinimal
	}
});

export const tooltipConfigState = $state({
	closeButton: 'ESC'
});
