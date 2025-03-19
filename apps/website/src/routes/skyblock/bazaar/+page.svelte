<script lang="ts">
	import type { BazaarType } from '$lib/types';
	import { clientFetch } from '$lib/other/clientFetch';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { bazaarState } from '$lib/states/bazaarState.svelte';
	import { toastActions } from '$lib/states/toastsState.svelte';
	import BazaarColumn from '$lib/components/BazaarColumn.svelte';
	import { BazaarItem } from '$lib/mongodb/BazaarItem';

	const onclick = async () => {
		let passable = true;
		const errors: string[] = [];

		function check(type: BazaarType) {
			bazaarState[type].forEach((e, i) => {
				// Name
				if (!cacheState.BAZAAR.find((ee) => ee.Name === e.Name)) {
					errors.push(`(${e.Name}) Name is invalid!`);
					passable = false;
				}

				// Price
				if (e.Price < 0 || e.Price > 1000000000) {
					errors.push(`(${e.Name}) Price must be between 0 and 1000000000!`);
					passable = false;
				}

				// OrderType
				if (e.OrderType !== 0 && e.OrderType !== 1) {
					errors.push(`(${e.Name}) OrderType is invalid!`);
					passable = false;
				}

				// RemovedAfter
				if (typeof e.RemovedAfter !== 'boolean') {
					errors.push(`(${e.Name}) RemovedAfter is invalid!`);
					passable = false;
				}
			});
		}

		check('BUYS');
		check('SELLS');

		for (const error of errors) {
			toastActions.addToast({ message: error, type: 'ERROR' });
		}

		if (!passable) {
			return;
		}

		const buys = bazaarState['BUYS'].map((e) => BazaarItem.ToType(e));
		const sells = bazaarState['SELLS'].map((e) => BazaarItem.ToType(e));

		await clientFetch('PUT=>/api/bazaar/buy', { items: buys });
		await clientFetch('PUT=>/api/bazaar/sell', { items: sells });
		toastActions.addToast({ message: 'Success!', type: 'NONE' });
	};
</script>

<div>
	<div class="flex flex-1">
		<BazaarColumn action="GET=>/api/bazaar/buy" actionDelete="DELETE=>/api/bazaar/buy" type="BUYS" />
		<BazaarColumn action="GET=>/api/bazaar/sell" actionDelete="DELETE=>/api/bazaar/sell" type="SELLS" />
	</div>
	<button class="button absolute-x-center absolute bottom-2" {onclick}>SUBMIT</button>
</div>
