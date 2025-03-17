<script lang="ts">
	import AuctionAddModal from '$lib/components/modals/AuctionAddModal.svelte';
	import AuctionTagsPanel from '$lib/components/panels/AuctionTagsPanel.svelte';
	import { clientFetch } from '$lib/other/clientFetch';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { onMount } from 'svelte';

	const { children } = $props();

	onMount(async () => {
		const response = await clientFetch('GET=>/api/auctions', {}, false);
		cacheState.AUCTIONS = (await response.JSON()).data;
	});
</script>

<AuctionAddModal />
<AuctionTagsPanel />
{@render children()}
