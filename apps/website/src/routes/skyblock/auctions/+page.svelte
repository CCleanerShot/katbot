<script lang="ts">
	import { AuctionItem } from '$lib/mongodb/AuctionItem';
	import { auctionState } from '$lib/states/auctionState.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';
	import AuctionColumn from '$lib/components/AuctionColumn.svelte';

	const onclick = async () => {
		const items = auctionState.BUYS.map((e) => AuctionItem.ToType(e));
		const response = await utilityClient.fetch('PUT=>/api/auctions/buy', { items: items }, true);
	};
</script>

<div class="flex flex-col gap-2">
	<AuctionColumn />
	<button class="button self-center" {onclick}>SUBMIT</button>
</div>
