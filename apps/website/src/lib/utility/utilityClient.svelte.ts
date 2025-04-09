import type { AuctionTag } from '$lib/mongodb/AuctionTag';

export const utilityClient = {
	/** used for making the first value of each array the name of the auction tags */
	groupTags: (tags: AuctionTag[] = []): string[][] => {
		const groups: Record<string, string[]> = {} as const;

		for (const tag of tags) {
			if (!groups[tag.Name]) {
				groups[tag.Name] = [tag.Value];
			} else {
				groups[tag.Name].push(tag.Value);
			}
		}

		const result: string[][] = [];

		for (const [key, values] of Object.entries(groups)) {
			result.push([key, ...values]);
		}

		return result;
	}
};
