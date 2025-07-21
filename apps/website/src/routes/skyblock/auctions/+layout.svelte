<script lang="ts">
	import { onMount } from 'svelte';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import AuctionTagsPanel from '$lib/components/panels/AuctionTagsPanel.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';

	const { children } = $props();

	onMount(async () => {
		const response = await utilityClient.fetch('GET=>/api/auctions', {}, false);
		cacheState.AUCTIONS = (await response.JSON()).data;
	});
</script>

<AuctionTagsPanel />
{@render children()}
