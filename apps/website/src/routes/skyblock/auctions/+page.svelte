<script lang="ts">
	import type { AuctionItem } from '$lib/mongodb/AuctionItem';
	import type { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
	import { clientFetch } from '$lib/other/clientFetch';
	import { auctionState } from '$lib/states/auctionState.svelte';
	import { modalState } from '$lib/states/modalState.svelte';
	import { panelState } from '$lib/states/panelState.svelte';

	const auctionItems = $derived(auctionState.BUYS);

	const onclick = async () => {
		const response = await clientFetch('GET=>/api/auctions/buy', {}, false);
		auctionState.BUYS = (await response.JSON()).data;
	};

	const onclickAdd = async () => {
		modalState.AuctionAddModal.isOpened = true;
	};

	const onclickTag = async (item: AuctionItem) => {
		auctionState.ITEM = item;
		panelState.AuctionTagsPanel.isOpened = true;
	};

	const onclickDelete = async (item: AuctionBuy, index: number) => {
		const response = await clientFetch('DELETE=>/api/auctions/buy', { ID: item.ID });

		if (response.ok) {
			auctionItems.splice(index, 1);
		}
	};
</script>

<div class="mt-2 flex flex-col items-center gap-1">
	<div class="flex gap-2">
		<button class="button w-32 text-center" {onclick}>REFRESH</button>
		<button class="button w-32 text-center" onclick={onclickAdd}>ADD</button>
	</div>
	<div class="flex flex-col items-center">
		{#if auctionItems.length > 0}
			<table class="table">
				<thead>
					<tr>
						<th>Name</th>
						<th>Price</th>
						<th>Retain</th>
						<th>Tags</th>
						<th></th>
					</tr>
				</thead>
				<tbody>
					{#each auctionItems as item, index (item.ID)}
						<tr class="group">
							<td>{item.Name}</td>
							<td>{item.Price}</td>
							<td>{item.RemovedAfter ? 'YES' : 'NO'}</td>
							<td class="group/inner relative cursor-pointer hover:font-bold" onclick={() => onclickTag(item)}>
								<span class="group-hover/inner:invisible">>></span>
								<span class="invisible absolute inset-0 text-white transition group-hover/inner:visible group-hover/inner:rotate-3">
									OPEN
								</span>
							</td>
							<td class="invisible px-2 group-hover:visible">
								<button class="remove-button hover:scale:105 button-border bg-red-500 px-2.5" onclick={() => onclickDelete(item, index)}>
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
