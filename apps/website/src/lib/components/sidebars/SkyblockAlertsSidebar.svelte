<script lang="ts">
	import Sidebar from './Sidebar.svelte';
	import { fade } from 'svelte/transition';
	import { utility } from '$lib/utility/utility';
	import { clientFetch } from '$lib/other/clientFetch';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import type { BazaarItem } from '$lib/mongodb/BazaarItem';
	import type { API_CONTRACTS } from '$lib/other/apiContracts';
	import { socketState } from '$lib/states/socketState.svelte';
	import { sidebarState } from '$lib/states/sidebarState.svelte';
	import { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
	import { utilityClient } from '$lib/utility/utilityClient.svelte';
	import { tooltipIsOpened, tooltipState } from '$lib/states/tooltipState.svelte';
	import type { AuctionsRouteProductMinimal, BazaarRouteProduct, BazaarSocketMessage } from '$lib/types';

	type DeleteRoutes = Extract<keyof typeof API_CONTRACTS, `${string}DELETE${string}`>;
	let hoveredItem: AuctionBuy | undefined = $state();
	let socketService = $derived(socketState.socketService);
	let currentState = $derived(sidebarState.SkyblockAlertsSidebar.items);

	const fadeTimer = 100;
	const actionType = {
		auctionBuy: {
			route: 'DELETE=>/api/auctions/buy',
			store: 'auctionSocketMessages'
		},
		bazaarBuy: {
			route: 'DELETE=>/api/bazaar/buy',
			store: 'bazaarSocketMessagesBuy'
		} as const,
		bazaarSell: {
			route: 'DELETE=>/api/bazaar/sell',
			store: 'bazaarSocketMessagesSell'
		} as const
	} as const satisfies Record<string, { route: DeleteRoutes; store: keyof typeof currentState }>;

	const acknowledgeItem = async (item: { ID: string; RemovedAfter: boolean }, index: number, action: keyof typeof actionType) => {
		// NOTE: not creating another route, since if u can delete manually already, there's no need for an extra route
		const { route, store } = actionType[action];
		let remove = true;

		if (item.RemovedAfter) {
			const response = await clientFetch(route, { ID: item.ID }, true);

			if (!response.ok) {
				remove = false;
			}
		}

		if (remove) {
			currentState[store].splice(index, 1);
		}
	};

	const onclick = () => {
		socketState.socketService?.InitalizeSocket();
	};

	const onclickMoreAuction = (buy: AuctionBuy, product: AuctionsRouteProductMinimal) => {
		tooltipState['AuctionProductInfoTooltip'].buy = buy;
		tooltipState['AuctionProductInfoTooltip'].product = product;
		tooltipIsOpened.current = 'AuctionProductInfoTooltip';
	};

	const onclickMoreBazaar = (item: BazaarItem, product: BazaarRouteProduct) => {
		tooltipState['BazaarProductInfoTooltip'].item = item;
		tooltipState['BazaarProductInfoTooltip'].product = product;
		tooltipIsOpened.current = 'BazaarProductInfoTooltip';
	};

	const onmouseenter = (buy: AuctionBuy) => {
		hoveredItem = buy;
		$state.snapshot(hoveredItem);
	};

	const onmouseleave = (buy: AuctionBuy) => {
		if (hoveredItem === buy) {
			hoveredItem = undefined;
		}
	};

	$effect(() => {
		$state.snapshot(hoveredItem);
	});
</script>

{#snippet title(props: SvelteHTMLElements['h3'])}
	<div class={['flex items-center justify-between gap-2 p-2', props.class]}>
		<h2 class={['text-center font-bold']}>Skyblock Feed</h2>
		<span class="font-xxx-small text-center">
			data is fetched every minute
			<br />
			give it time to update!
		</span>
		{#if socketService?.retrying}
			<button class="button relative" disabled>
				<span>RETRYING ({socketService.retries})</span>
			</button>
		{:else}
			<button class="button group relative" disabled={!!socketState.socketService?.socket} {onclick}>
				<span class="group-hover:invisible">RECONNECT</span>
				<span class={[socketService?.socket ? "group-hover:visible group-hover:rotate-3" : "" , "invisible absolute inset-0 pt-[4px] transition"]}>IT'S ON</span>
			</button>
		{/if}
	</div>
{/snippet}

{#snippet bazaarTable(items: BazaarSocketMessage[], title: string, action: Extract<keyof typeof actionType, `${string}bazaar${string}`>)}
	<div class="flex flex-col items-center" out:fade={{ duration: fadeTimer }}>
		<h5>{title}</h5>
		<table class="font-x-small-recursive table-right-1">
			<thead>
				<tr>
					<th>Name</th>
					<th>Requested Price</th>
					<th>Live Price</th>
					<th>More</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				{#each items as item, index (index)}
					<tr out:fade={{ duration: fadeTimer }}>
						<td>{item.RequestedItem.Name}</td>
						<td class="text-right">{utility.formatNumber(item.RequestedItem.Price)}</td>
						<td class="text-right">
							{utility.formatNumber(
								action === 'bazaarBuy'
									? Math.floor(item.LiveSummary.sell_summary[0].pricePerUnit)
									: Math.floor(item.LiveSummary.buy_summary[0].pricePerUnit)
							)}
						</td>
						<td
							class="group/inner relative cursor-pointer text-center hover:font-bold"
							onclick={() => onclickMoreBazaar(item.RequestedItem, item.LiveSummary)}
						>
							<span class="group-hover/inner:invisible">>></span>
							<span class="invisible absolute inset-0 text-white transition group-hover/inner:visible group-hover/inner:rotate-3">
								MORE
							</span>
						</td>
						<td class="px-1">
							<button
								class="bg-green-500 px-1 transition hover:translate-x-0.5 hover:bg-black hover:text-white"
								onclick={() => acknowledgeItem(item.RequestedItem, index, action)}
							>
								✔
							</button>
						</td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>
{/snippet}

<Sidebar sidebar="SkyblockAlertsSidebar" {title}>
	<div class="pl-2">
		{#if currentState.auctionSocketMessages.length}
			<div class="auctions-container flex flex-col items-center" out:fade={{ duration: 100 }}>
				<h4>Auctions</h4>
				<div class="flex flex-col items-center">
					<h5>Wanted Items</h5>
					<table class="font-xx-small-recursive table-right-1">
						<thead>
							<tr>
								<th>Name</th>
								<th>Price</th>
								<th>Tags</th>
								<th></th>
							</tr>
						</thead>
						<tbody>
							{#each currentState.auctionSocketMessages as message, index (index)}
								<tr
									class="{message.RequestedItem.ID}-{index}"
									out:fade={{ duration: 100 }}
									onmouseleave={() => onmouseleave(message.RequestedItem)}
									onmouseenter={() => onmouseenter(message.RequestedItem)}
								>
									<td>{message.RequestedItem.Name}</td>
									<td class="text-right">{utility.formatNumber(message.RequestedItem.Price)}</td>
									<td>
										{#each utilityClient.groupTags(message.RequestedItem.AuctionTags) as tags, index2 (index2)}
											<span class="overflow-x-auto">
												{tags[0]}:
												{#each tags.slice(1) as entries, index3}
													({entries})
												{/each}
											</span>
										{/each}
									</td>
									<td class="px-1">
										<button
											class="bg-green-500 px-1 transition hover:translate-x-0.5 hover:bg-black hover:text-white"
											onclick={() => acknowledgeItem(message.RequestedItem, index, 'auctionBuy')}
										>
											✔
										</button>
									</td>
								</tr>
							{/each}
						</tbody>
					</table>
				</div>
				<h5>Live Items</h5>
				<table class="font-xx-small-recursive table">
					<thead>
						<tr>
							<th>Name</th>
							<th>Live Price</th>
							<th>Live Tags</th>
							<th>Info</th>
						</tr>
					</thead>
					<tbody>
						{#each currentState.auctionSocketMessages as message, index (index)}
							{#each message.LiveItems as liveItem, index2 (index2)}
								<tr
									class={[hoveredItem ? (AuctionBuy.Equals(hoveredItem, message.RequestedItem) ? 'hovered-item' : '') : '']}
									out:fade={{ duration: 100 }}
								>
									<td>{message.RequestedItem.Name}</td>
									<td class="text-right"
										>{utility.formatNumber(Math.max(Number(liveItem.highest_bid_amount), Number(liveItem.starting_bid)))}</td
									>
									<td>
										{#each utilityClient.groupTags(liveItem.AuctionTags.filter( (e) => message.RequestedItem.AuctionTags.find((ee) => ee.Name === e.Name) )) as tags, aIndex}
											<span class="overflow-x-auto">
												{tags[0]}:
												{#each tags.slice(1) as entries, eIndex}
													({entries})
												{/each}
											</span>
										{/each}
									</td>
									<td
										class="group/inner relative cursor-pointer text-center hover:font-bold"
										onclick={() => onclickMoreAuction(message.RequestedItem, liveItem)}
									>
										<span class="group-hover/inner:invisible">>></span>
										<span class="invisible absolute inset-0 text-white transition group-hover/inner:visible group-hover/inner:rotate-3">
											MORE
										</span>
									</td>
								</tr>
							{/each}
						{/each}
					</tbody>
				</table>
			</div>
		{/if}
		{#if currentState.bazaarSocketMessagesBuy.length || currentState.bazaarSocketMessagesSell.length || currentState.auctionSocketMessages.length}
			<section class="mt-2">
				<h4>Bazaar</h4>
				{#if currentState.bazaarSocketMessagesBuy.length}
					{@render bazaarTable(currentState.bazaarSocketMessagesBuy, 'BUYS', 'bazaarBuy')}
				{/if}
				{#if currentState.bazaarSocketMessagesSell.length}
					{@render bazaarTable(currentState.bazaarSocketMessagesSell, 'SELLS', 'bazaarSell')}
				{/if}
			</section>
		{/if}
		{#if !currentState.auctionSocketMessages.length && !currentState.bazaarSocketMessagesBuy.length && !currentState.bazaarSocketMessagesSell.length}
			<section class="flex">
				<span class="font-small text-center">New updates will appear here!</span>
			</section>
		{/if}
	</div>
</Sidebar>

<style>
	h4 {
		text-decoration: underline;
		text-align: center;
	}

	.auctions-container * {
		text-wrap: nowrap;
	}

	.hovered-item {
		background-color: var(--color-secondary);
	}
</style>
