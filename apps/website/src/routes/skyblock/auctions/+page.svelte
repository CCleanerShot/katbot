<script lang="ts">
	import { clientFetch } from '$lib/other/clientFetch';
	import { auctionState } from '$lib/states/auctionState.svelte';

	const auctionItems = $derived(auctionState.BUYS);

	const onclick = async () => {
		const response = await clientFetch('GET=>/api/auctions/buy', {}, false);
		auctionState.BUYS = (await response.JSON()).data;
	};

	const onclickAdd = async () => {};

	$inspect(auctionItems);
</script>

<div class="mt-2 flex flex-col items-center gap-1">
	<div class="flex gap-2">
		<button class="button w-32 text-center" {onclick}>REFRESH</button>
		<button class="button w-32 text-center" onclick={onclickAdd}>ADD</button>
	</div>
	<div class="flex flex-col items-center">
		{#if auctionItems.length > 0}
			<table class="table text-[8px]">
				<thead>
					<tr>
						<th>Name</th>
						<th>Price</th>
						<th>Retain</th>
						<th>Tags</th>
					</tr>
				</thead>
				<tbody>
					{#each auctionItems as item (item.ID)}
						<tr class="group">
							<td>{item.Name}</td>
							<td>{item.Price}</td>
							<td>{item.RemovedAfter}</td>
							<td>
								<select>
									{#each item.AuctionTags as tag (tag.Name)}
										<option class="w-12" value={tag.Name}>{tag.Name}</option>
									{/each}
								</select>
							</td>
						</tr>
					{/each}
				</tbody>
			</table>
		{/if}
	</div>
</div>
