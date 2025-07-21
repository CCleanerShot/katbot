import type { AuctionTag } from '$lib/mongodb/AuctionTag';
import { API_CONTRACTS } from '$lib/common/apiContracts';
import { fetchState } from '$lib/states/fetchState.svelte';
import { toastActions } from '$lib/states/toastsState.svelte';
import { utility } from '$lib/common/utility';

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
	},
	/** Wrapper function for `utility.fetch()`. Can also emits a toast to the page, from the optional parameter, `useToast`. */
	fetch: async <T extends keyof typeof API_CONTRACTS>(
		request: T,
		params: (typeof API_CONTRACTS)[T]['params'],
		useToast: boolean = false
	): ReturnType<typeof utility.fetch> => {
		fetchState.status = 'loading';

		const result = await utility.fetch(request, params);

		if (useToast) {
			switch (result.ok) {
				case true:
					toastActions.addToast({ message: 'Success!', type: 'NONE' });
					fetchState.status = 'none';
					break;
				case false:
					let message;

					try {
						message = ((await (result as any).JSON()) as { message: string }).message;

						if (message.includes('Internal Error')) {
							message = 'Server unexpected failed! Contact me if the issue persists.';
						}
					} catch (err) {
						message = 'Server unexpected failed! Contact me if the issue persists.';
					}

					toastActions.addToast({ message: message, type: 'ERROR' });
					fetchState.status = 'error';

					// TODO: get delay from config
					setTimeout(() => {
						fetchState.status = 'none';
					}, 4000);

					break;
			}
		}

		fetchState.status = 'none';
		return result;
	}
};
