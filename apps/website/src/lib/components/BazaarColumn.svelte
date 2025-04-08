<script lang="ts">
	import { OrderType } from '$lib/enums';
	import type { BazaarType } from '$lib/types';
	import { BazaarItem } from '$lib/mongodb/BazaarItem';
	import { clientFetch } from '$lib/other/clientFetch';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import type { API_CONTRACTS } from '$lib/other/apiContracts';
	import { bazaarState } from '$lib/states/bazaarState.svelte';
	import AutoComplete from './autocompletes/AutoComplete.svelte';

	type Props = {
		action: Extract<keyof typeof API_CONTRACTS, `GET${string}/bazaar/${string}`>;
		actionDelete: Extract<keyof typeof API_CONTRACTS, `DELETE${string}/bazaar/${string}`>;
		type: BazaarType;
	};

	class Item {
		Name: string = $state('');
		ID: string = $derived(cacheState.BAZAAR.find((e) => e.Name === this.Name)?.ID ?? '');
		OrderType: OrderType = $state(OrderType.ORDER);
		Price: bigint = $state(0n);
		RemovedAfter = $state(true);
		UserId: bigint = $state(0n);

		constructor(item?: BazaarItem) {
			if (!item) {
				return;
			}

			const { ID, Name, OrderType, Price, RemovedAfter, UserId } = item;
			this.Name = Name;
			this.OrderType = OrderType;
			this.Price = Price;
			this.RemovedAfter = RemovedAfter;
			this.UserId = UserId;
		}
	}

	const { action, actionDelete, type }: Props = $props();

	const onclick = async () => {
		const response = await clientFetch(action, {});
		const json = await response.JSON();
		bazaarState[type] = json.data.map((e) => new Item(e));
	};

	const onclickAdd = () => {
		bazaarState[type].push(BazaarItem.Empty());
	};

	const onclickDelete = async (item: BazaarItem, index: number) => {
		if (item.UserId === 0n) {
			bazaarState[type].splice(index, 1);
			return;
		}

		const response = await clientFetch(actionDelete, { ID: item.ID });

		if (response.ok) {
			bazaarState[type].splice(index, 1);
		}
	};
</script>

<div class="flex h-full flex-1 flex-col border-black p-2 not-last:border-r-2">
	<div class="flex justify-center gap-2">
		<button class="button group relative w-28" {onclick}>
			<span class="group-hover:invisible">{type}</span>
			<span class="absolute-center invisible group-hover:visible">REFRESH</span>
		</button>
		<button class="button w-28" onclick={onclickAdd}>
			<span>ADD</span>
		</button>
	</div>
	<div class="flex justify-center p-2">
		{#if bazaarState[type].length != 0}
			<table class="font-x-small-recursive table border-black">
				<thead>
					<tr>
						<th>Name</th>
						<th>Price</th>
						<th>OrderType</th>
						<th>Retain</th>
						<th></th>
					</tr>
				</thead>
				<tbody>
					{#each bazaarState[type] as item, index (`${item.ID}-${index}`)}
						<tr class="group">
							<td>
								<AutoComplete
									bind:array={cacheState['BAZAAR']}
									bind:value={item.Name}
									updateKey={'Name'}
									updateObj={bazaarState[type][index]}
									inputProps={{ placeholder: 'Type Here to Add...' }}
								/>
							</td>
							<td><input bind:value={item.Price} type="number" class="w-20 text-right" min="0" max="1000000000" /></td>
							<td>
								<select bind:value={item.OrderType}>
									<option value={0}>INSTA</option>
									<option value={1}>ORDER</option>
								</select>
							</td>
							<td>
								<select bind:value={item.RemovedAfter}>
									<option value={false}>YES</option>
									<option value={true}>NO</option>
								</select>
							</td>
							<td class="invisible px-1.5 group-hover:visible">
								<button class="remove-button hover:scale:105 button-border bg-red-500 px-1.5" onclick={() => onclickDelete(item, index)}>
									X
								</button>
							</td>
						</tr>
					{/each}
				</tbody>
			</table>
		{/if}
	</div>
</div>
