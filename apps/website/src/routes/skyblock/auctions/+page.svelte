<script lang="ts">
	import AuctionColumn from '$lib/components/AuctionColumn.svelte';
	import { AuctionItem } from '$lib/mongodb/AuctionItem';
	import { clientFetch } from '$lib/other/clientFetch';
	import { auctionState } from '$lib/states/auctionState.svelte';

	const onclick = async () => {
		const items = auctionState.BUYS.map((e) => AuctionItem.ToType(e));
		const response = await clientFetch('PUT=>/api/auctions/buy', { items: items }, true);
	};
</script>

<div class="flex flex-col gap-2">
	<AuctionColumn />
	<button class="button self-center" {onclick}>SUBMIT</button>
</div>
