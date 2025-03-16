import { TagType } from '$lib/enums';

export type AuctionTag = {
	/** The name of the tag. */
	Name: string;
	/** The type of tag. */
	Type: TagType;
	/** The value for this tag. */
	Value: string;
};
