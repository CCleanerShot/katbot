import { AuctionItem } from '$lib/mongodb/AuctionItem';
import type { AuctionTag } from '$lib/mongodb/AuctionTag';

export const panelState = $state({
	AuctionTagsPanel: {
		isOpened: false as boolean,
		item: { AuctionTags: [], ID: '', Name: '', Price: 0n, RemovedAfter: false, UserId: 0n } as AuctionItem,
		tag: undefined as AuctionTag | undefined
	}
});
