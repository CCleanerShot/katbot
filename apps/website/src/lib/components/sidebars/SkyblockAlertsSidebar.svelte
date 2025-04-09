<script lang="ts">
	import Sidebar from './Sidebar.svelte';
	import { fade } from 'svelte/transition';
	import { clientFetch } from '$lib/other/clientFetch';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import type { BazaarItem } from '$lib/mongodb/BazaarItem';
	import type { API_CONTRACTS } from '$lib/other/apiContracts';
	import { socketState } from '$lib/states/socketState.svelte';
	import type { AuctionsRouteProductMinimal } from '$lib/types';
	import { sidebarState } from '$lib/states/sidebarState.svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';
	import {utilityClient} from "$lib/utility/utilityClient.svelte";
	import type { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
	
	type DeleteRoutes = Extract<keyof typeof API_CONTRACTS, `${string}DELETE${string}`>;
	let socketService = $derived(socketState.socketService);
	let state = $derived(sidebarState.SkyblockAlertsSidebar.items);

	const fadeTimer = 100;
	const actionType = {
		auctionBuy: {
			route: 'DELETE=>/api/auctions/buy',
			store: 'auctionSocketMessages'
		},
		bazaarBuy: {
			route: 'DELETE=>/api/bazaar/buy',
			store: 'bazaarBuys'
		} as const,
		bazaarSell: {
			route: 'DELETE=>/api/bazaar/sell',
			store: 'bazaarSells'
		} as const
	} as const satisfies Record<string, { route: DeleteRoutes; store: keyof typeof state }>;

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
			state[store].splice(index, 1);
		}
	};

	const onclick = () => {
		socketState.socketService?.InitalizeSocket();
	};

	const onclickMore = (buy: AuctionBuy, product: AuctionsRouteProductMinimal) => {
		console.log(buy, product)
		tooltipState['AuctionProductInfoTooltip'].buy = buy;
		tooltipState['AuctionProductInfoTooltip'].product = product;
		tooltipState['AuctionProductInfoTooltip'].isOpened = true;
	};
</script>

{#snippet title(props: SvelteHTMLElements['h3'])}
	<div class={['flex items-center justify-between gap-2 p-2', props.class]}>
		<span class={['text-center font-bold']}>Skyblock Feed</span>
		{#if socketService?.retrying}
			<button class="button relative" disabled>
				<span>RETRYING ({socketService.retries})</span>
			</button>
		{:else}
			<button class="button group relative" disabled={!!socketState.socketService?.socket} {onclick}>
				<span class="group-hover:invisible">RECONNECT</span>
				<span class="invisible absolute inset-0 pt-[4px] transition group-hover:visible group-hover:rotate-3">IT'S ON</span>
			</button>
		{/if}
	</div>
{/snippet}

{#snippet bazaarTable(items: BazaarItem[], title: string, action: keyof typeof actionType)}
	<div class="flex flex-col items-center" out:fade={{ duration: fadeTimer }}>
		<h5>{title}</h5>
		<table class="font-x-small-recursive table">
			<thead>
				<tr>
					<th>Name</th>
					<th>Price</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				{#each items as item, index (index)}
					<tr out:fade={{ duration: fadeTimer }}>
						<td>{item.Name}</td>
						<td>{item.Price}</td>
						<td class="px-1">
							<button
								class="bg-green-500 px-1 transition hover:translate-x-0.5 hover:bg-black hover:text-white"
								onclick={() => acknowledgeItem(item, index, action)}
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
		{#if state.auctionSocketMessages.length}
			<div class="auctions-container flex flex-col items-center" out:fade={{ duration: 100 }}>
				<h5>Auctions</h5>
				<table class="font-xx-small-recursive table">
					<thead>
						<tr>
							<th>Name</th>
							<th>Live Price</th>
							<th>Live Tags</th>
							<th>Info</th>
							<th></th>
						</tr>
					</thead>
					<tbody>
						{#each state.auctionSocketMessages as message, index (index)}

								{#each message.LiveItems as liveItem, index2 (index2)}
								<tr class="{message.BuyItem.ID}-{index}" out:fade={{ duration: 100 }}>
									<td>{message.BuyItem.Name}</td>
									<td class="text-right">{Math.max(Number(liveItem.highest_bid_amount), Number(liveItem.starting_bid))}</td>
									<td>
										{#each utilityClient.groupTags(liveItem.AuctionTags.filter( (e) => message.BuyItem.AuctionTags.find((ee) => ee.Name === e.Name) )) as tags, aIndex}
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
									onclick={() => onclickMore(message.BuyItem, liveItem)}
									>
									<span class="group-hover/inner:invisible">>></span>
									<span class="invisible absolute inset-0 text-white transition group-hover/inner:visible group-hover/inner:rotate-3">
										MORE
									</span>
								</td>
								<td class="px-1">
									<button
									class="bg-green-500 px-1 transition hover:translate-x-0.5 hover:bg-black hover:text-white"
									onclick={() => acknowledgeItem(message.BuyItem, index, 'auctionBuy')}
									>
									✔
								</button>
							</td>
						</tr>
						{/each}
						{/each}
					</tbody>
				</table>
			</div>
		{/if}
		{#if state.bazaarBuys.length || state.bazaarSells.length || state.auctionSocketMessages.length}
			<section class="mt-2">
				<h4>Bazaar</h4>
				{#if state.bazaarBuys.length}
					{@render bazaarTable(state.bazaarBuys, 'BUYS', 'bazaarBuy')}
				{/if}
				{#if state.bazaarSells.length}
					{@render bazaarTable(state.bazaarSells, 'SELLS', 'bazaarSell')}
				{/if}
			</section>
		{/if}
		{#if !state.auctionSocketMessages.length && !state.bazaarBuys.length && !state.bazaarSells.length}
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

	.super-small {
		font-size: 0.2rem;
	}
</style>
