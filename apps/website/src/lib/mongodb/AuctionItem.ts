import { cacheState } from '$lib/states/cacheState.svelte';
import type { AuctionTag } from './AuctionTag';

export class AuctionItem {
	/** List of AuctioNTags that the tracked item should have. */
	AuctionTags: AuctionTag[];
	/** The Hypixel ID of the item. */
	ID: string;
	/** The name of the item. */
	Name: string;
	/** The price to start evaluating the threshold for alerts. */
	Price: bigint;
	/** Whether or not to remove the tracked item after successfully found. */
	RemovedAfter: boolean;
	/** The Discord ID of the user that submitted the tracking item. */
	UserId: bigint;

	constructor(_AuctionTags: AuctionTag[], _ID: string, _Name: string, _Price: bigint, _RemovedAfter: boolean, _UserId: bigint) {
		this.AuctionTags = _AuctionTags;
		this.ID = _ID;
		this.Name = _Name;
		this.Price = _Price;
		this.RemovedAfter = _RemovedAfter;
		this.UserId = _UserId;
	}

	static ToType(item: AuctionItem): AuctionItem {
		const { AuctionTags, Name, Price, RemovedAfter, UserId } = item;
		const ID = cacheState.AUCTIONS.items.find((e) => e.Name === item.Name)?.ID!;
		return { AuctionTags, ID, Name, Price, RemovedAfter, UserId } as AuctionItem;
	}
}
