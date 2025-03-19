<script lang="ts">
	import { onMount } from 'svelte';
	import { clientFetch } from '$lib/other/clientFetch';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import AuctionTagsPanel from '$lib/components/panels/AuctionTagsPanel.svelte';

	const { children } = $props();

	onMount(async () => {
		const response = await clientFetch('GET=>/api/auctions', {}, false);
		cacheState.AUCTIONS = (await response.JSON()).data;
	});
</script>

<AuctionTagsPanel />
{@render children()}
