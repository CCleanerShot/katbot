<script lang="ts">
	import Tooltip from './Tooltip.svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';

	let { buy, product } = $derived(tooltipState['AuctionProductInfoTooltip']);

</script>

<Tooltip tooltip="AuctionProductInfoTooltip">
	<div class="flex flex-col font-xx-small-recursive p-1 gap-1">
		<section >
			<h4>Wanted Item</h4>
			<div class="flex flex-col">
				<table>
					<tbody>
						<tr>
							<th>Name</th>
							<td>{buy.Name}</td>
						</tr>
						<tr>
							<th>Price</th>
							<td>{buy.Price}</td>
						</tr>
						<tr>
							<th>Tags</th>
							<td>
							{#each utilityClient.groupTags(buy.AuctionTags) as tags}
								<div class="overflow-x-auto">
									{tags[0]}:
									{#each tags.slice(1) as entries, eIndex}
										({entries})
									{/each}
								</div>
							{/each}
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</section>
		<section>
			<h4>Live Item</h4>
			<div class="flex flex-col">
				<table>
					<tbody>
						<tr>
							<th>Name</th>
							<td>{product.ITEM_NAME}</td>
						</tr>
						<tr>
							<th>Price</th>
							<td>{Math.max(Number(product.highest_bid_amount), Number(product.starting_bid))}</td>
						</tr>
						<tr>
							<th>Tags</th>
							<td>
								{#each utilityClient.groupTags(product.AuctionTags) as tags}
									<div class="overflow-x-auto not-last:mr-1">
										{tags[0]}:
										{#each tags.slice(1) as entries, eIndex}
											({entries})
										{/each}
									</div>
								{/each}
							</td>
						</tr>
						<tr>
							<th>Seller</th>
							<td>{product.auctioneer}</td>
						</tr>
					</tbody>
				</table>
			</div>
		</section>
	</div>
</Tooltip>

<style>
	section {
		padding: 0.25rem;
	}

	section h4 {
		text-align: center;
		text-decoration: underline;
	}

	table tr > * {
		border: 1px solid black;
		padding: 0.1rem;
	}
</style>