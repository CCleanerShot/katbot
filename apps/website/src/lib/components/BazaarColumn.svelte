<script lang="ts">
	import { BazaarItem } from '$lib/mongodb/BazaarItem';
	import type { API_CONTRACTS } from '$lib/other/apiContracts';
	import { clientFetch } from '$lib/other/clientFetch';
	import { bazaarState } from '$lib/states/bazaarState.svelte';
	import { modalState } from '$lib/states/modalState.svelte';
	import type { BazaarType } from '$lib/types';

	type Props = {
		action: Extract<keyof typeof API_CONTRACTS, `GET${string}/bazaar/${string}`>;
		actionAdd: Extract<keyof typeof API_CONTRACTS, `POST${string}/bazaar/${string}`>;
		actionDelete: Extract<keyof typeof API_CONTRACTS, `DELETE${string}/bazaar/${string}`>;
		type: BazaarType;
	};

	const { action, actionAdd, actionDelete, type }: Props = $props();

	const onclick = async () => {
		const response = await clientFetch(action, {});
		const json = await response.JSON();
		bazaarState[type] = json.data.map((e) => BazaarItem.ToClass(e));
	};

	const deleteOnClick = async (item: BazaarItem, index: number) => {
		const response = await clientFetch(actionDelete, { ID: item.ID });

		if (response.ok) {
			bazaarState[type].splice(index, 1);
		}
	};

	const addOnClick = () => {
		modalState.BazaarAddModal.action = actionAdd;
		modalState.BazaarAddModal.type = type;
		modalState.BazaarAddModal.isOpened = true;
	};
</script>

<div class="not-last:border-r-2 flex h-full flex-1 flex-col border-black p-1">
	<div class="flex justify-center gap-2">
		<button class="button group relative w-20" {onclick}>
			<span class="group-hover:invisible">{type}</span>
			<span class="absolute-center invisible group-hover:visible">REFRESH</span>
		</button>
		<button class="button w-20" onclick={addOnClick}>
			<span>ADD</span>
		</button>
	</div>
	<div class="flex justify-center p-2">
		{#if bazaarState[type].length != 0}
			<table class="table border-black text-[8px]">
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
					{#each bazaarState[type] as item, index (item.Name)}
						<tr class="group">
							<td>{item.Name}</td>
							<td>{item.Price}</td>
							<td>{BazaarItem.OrderTypeString(item)}</td>
							<td>{item.RemovedAfter ? 'YES' : 'NO'}</td>
							<td class="invisible px-1 group-hover:visible">
								<button class="remove-button hover:scale:105 button-border bg-red-500 px-1" onclick={() => deleteOnClick(item, index)}>
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

<style>
</style>
