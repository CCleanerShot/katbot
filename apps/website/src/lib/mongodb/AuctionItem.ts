import type { AuctionTag } from './AuctionTag';

export type AuctionItem = {
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
};
