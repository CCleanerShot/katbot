<script lang="ts">
	import Tooltip from './Tooltip.svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';

	let { buy, product } = $state(tooltipState['AuctionProductInfoTooltip']);
</script>

<Tooltip tooltip="AuctionProductInfoTooltip">
	<section id="your-item-section" class="flex flex-col">
		<h3>Your Item:</h3>
		<div>{buy.Name}</div>
		<div>{buy}</div>
	</section>
	<section id="live-item-section">
		<h3>Live Item:</h3>
		<div></div>
	</section>
	{buy.Name}
</Tooltip>

<style>
	section {
		border-width: 0.1rem;
		border-radius: 0.1rem;
	}

	#live-item-section {
		border-color: var(--color-secondary);
	}

	#your-item-section {
		border-color: var(--color-primary);
	}
</style>