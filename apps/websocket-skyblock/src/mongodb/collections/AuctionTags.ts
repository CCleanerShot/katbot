import type { TagType } from '$lib/enums';

export type AuctionTags = {
	/** The name of the tag. */
	Name: string;
	/** The type of tag. */
	Type: TagType;
	/** A list of values this tag has been seen with. */
	Values: string[];
};
