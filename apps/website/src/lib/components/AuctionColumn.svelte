<script lang="ts">
	import { clientFetch } from '$lib/other/clientFetch';
	import { panelState } from '$lib/states/panelState.svelte';
	import type { AuctionItem } from '$lib/mongodb/AuctionItem';
	import { auctionState } from '$lib/states/auctionState.svelte';
	import type { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
	import Autocomplete from './autocompletes/Autocomplete.svelte';
	import { cacheState } from '$lib/states/cacheState.svelte';

	class Item {
		AuctionTags: AuctionItem['AuctionTags'] = $state([]);
		ID: AuctionItem['ID'] = $state('');
		Name: AuctionItem['Name'] = $state('');
		Price: AuctionItem['Price'] = $state(0n);
		RemovedAfter: AuctionItem['RemovedAfter'] = $state(false);
		UserId: AuctionItem['UserId'] = $state(0n);

		constructor(item?: AuctionItem) {
			if (!item) {
				return;
			}

			const { AuctionTags, ID, Name, Price, RemovedAfter, UserId } = item;
			this.AuctionTags = AuctionTags;
			this.ID = ID;
			this.Name = Name;
			this.Price = Price;
			this.RemovedAfter = RemovedAfter;
			this.UserId = UserId;
		}
	}

	const onclick = async () => {
		const response = await clientFetch('GET=>/api/auctions/buy', {}, false);
		const json = await response.JSON();
		auctionState.BUYS = json.data.map((e) => new Item(e));
	};

	const onclickAdd = async () => {
		auctionState.BUYS.push(new Item());
	};

	const onclickTag = async (item: AuctionItem) => {
		panelState.AuctionTagsPanel.item = item;
		panelState.AuctionTagsPanel.isOpened = true;
	};

	const onclickDelete = async (item: AuctionBuy, index: number) => {
		const response = await clientFetch('DELETE=>/api/auctions/buy', { ID: item.ID });

		if (response.ok) {
			auctionState.BUYS.splice(index, 1);
		}
	};
</script>

<div class="mt-2 flex flex-col items-center gap-2">
	<div class="flex gap-2">
		<button class="button w-32 text-center" {onclick}>REFRESH</button>
		<button class="button w-32 text-center" onclick={onclickAdd}>ADD</button>
	</div>
	<div class="flex flex-col items-center">
		{#if auctionState.BUYS.length > 0}
			<table class="font-small-recursive table">
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
					{#each auctionState.BUYS as item, index (item.ID + index)}
						<tr class="group">
							<td>
								<Autocomplete
									array={cacheState.AUCTIONS.items}
									bind:value={item.Name}
									updateObj={item}
									updateKey={'Name'}
									inputProps={{ class: 'w-72', type: 'text' }}
								/>
							</td>
							<td><input bind:value={item.Price} type="number" min="0" /></td>
							<td>
								<select bind:value={item.RemovedAfter}>
									<option value={true}>YES</option>
									<option value={false}>NO</option>
								</select>
							</td>
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
