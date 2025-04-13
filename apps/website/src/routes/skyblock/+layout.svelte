<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import RoutePage from '$lib/components/RoutePage.svelte';
	import { socketState } from '$lib/states/socketState.svelte';
	import { WebsocketService } from '$lib/classes/WebsocketService.svelte';
	import { PUBLIC_PREFIX_WEBSOCKET, PUBLIC_DOMAIN } from '$env/static/public';
	import SkyblockAlertsSidebar from '$lib/components/sidebars/SkyblockAlertsSidebar.svelte';
	import AuctionProductInfoTooltip from '$lib/components/tooltips/AuctionProductInfoTooltip.svelte';
	import BazaarProductInfoTooltip from '$lib/components/tooltips/BazaarProductInfoTooltip.svelte';
	import { utility } from '$lib/utility/utility';

	const { children } = $props();

	onMount(() => {
		const url = `${utility.getProtocol("ws")}://${PUBLIC_PREFIX_WEBSOCKET}.${PUBLIC_DOMAIN}`;
		socketState.socketService = new WebsocketService(url);
	});
</script>

{#snippet link(className: string)}
	<span class={['font-xx-small-recursive rotate-3 whitespace-nowrap transition hover:rotate-2', className]}>
		check
		<a
			class="font-x-small not-visited:text-blue-500 visited:text-purple-500 hover:underline"
			href="https://www.skyblock.bz"
			target="_blank"
		>
			https://www.skyblock.bz
		</a>
		for prices.
	</span>
{/snippet}

<div>
	<AuctionProductInfoTooltip />
	<BazaarProductInfoTooltip />
	<SkyblockAlertsSidebar />
	<div class="flex items-center justify-center gap-2 border-b-2 border-black py-2">
		{#if page.route.id?.includes('bazaar')}
			{@render link('invisible')}
		{/if}
		<RoutePage className="w-36" route="/skyblock/auctions" title="Auctions" />
		<RoutePage className="w-36" route="/skyblock/bazaar" title="Bazaar" />
		{#if page.route.id?.includes('bazaar')}
			{@render link('')}
		{/if}
	</div>
	{@render children()}
</div>
