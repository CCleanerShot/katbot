<script lang="ts">
	import Tooltip from './Tooltip.svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';

	let { buy, product } = $state(tooltipState['AuctionProductInfoTooltip']);
</script>

<Tooltip tooltip="AuctionProductInfoTooltip">test</Tooltip>
