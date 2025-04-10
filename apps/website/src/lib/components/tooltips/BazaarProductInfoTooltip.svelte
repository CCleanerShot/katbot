<script lang="ts">
	import Tooltip from './Tooltip.svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';
	import type { BazaarRouteSummaryItem } from '$lib/types';

	let { item, product, type } = $derived(tooltipState['BazaarProductInfoTooltip']);
	let summaries: BazaarRouteSummaryItem[] = $derived(type === 'BUY' ? product.buy_summary : product.sell_summary);
</script>

<Tooltip tooltip="BazaarProductInfoTooltip">
	<div class="font-xx-small-recursive flex flex-col gap-1 p-1">
		<section>
			<h4>Wanted Item</h4>
			<table>
				<thead>
					<tr>
						<th>Name</th>
						<th>Price</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td>{item.Name}</td>
						<td>{item.Price}</td>
					</tr>
				</tbody>
			</table>
		</section>
		<section>
			<h4>Live Orders</h4>
			<div class="max-h-20 overflow-y-auto">

				<table class="h-1">
					<thead>
						<tr>
							<th>Price</th>
							<th>Size</th>
							<th>Amount</th>
						</tr>
					</thead>
					<tbody class="h-1">
						{#each summaries as summary, index}
						<tr>
							<td>{summary.pricePerUnit}</td>
							<td>{summary.amount}</td>
							<td>{summary.orders}</td>
						</tr>
						{/each}
					</tbody>
				</table>
			</div>
			</section>
		</div>
	</Tooltip>
	
<style>
	section {
		padding: 0.25rem;
		display: flex;
		flex-direction: column;
		align-items: center;
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
