<script lang="ts">
	import AuctionAddModal from '$lib/components/modals/AuctionAddModal.svelte';
	import AuctionTagPanel from '$lib/components/panels/AuctionTagPanel.svelte';
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
<AuctionTagPanel />
{@render children()}
