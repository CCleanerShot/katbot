<script lang="ts">
	import Sidebar from './Sidebar.svelte';
	import { fade } from 'svelte/transition';
	import { clientFetch } from '$lib/other/clientFetch';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import type { BazaarItem } from '$lib/mongodb/BazaarItem';
	import type { API_CONTRACTS } from '$lib/other/apiContracts';
	import { sidebarState } from '$lib/states/sidebarState.svelte';
	import { socketState } from '$lib/states/socketState.svelte';
	import type { AuctionTag } from '$lib/mongodb/AuctionTag';

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

	/** first value represents the key */
	const groupTags = (tags: AuctionTag[]): string[][] => {
		const groups: Record<string, string[]> = {} as const;

		for (const tag of tags) {
			if (!groups[tag.Name]) {
				groups[tag.Name] = [tag.Value];
			} else {
				groups[tag.Name].push(tag.Value);
			}
		}

		const result: string[][] = [];

		for (const [key, values] of Object.entries(groups)) {
			result.push([key, ...values]);
		}

		return result;
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
						<td class="px-1.5">
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
	<div class="px-2">
		{#if state.auctionSocketMessages.length}
			<div class="flex flex-col items-center auctions-container" out:fade={{ duration: 100 }}>
				<h5>Auctions</h5>
				<table class="font-xx-small-recursive table">
					<thead>
						<tr>
							<th>Name</th>
							<th>Price</th>
							<th>Tags</th>
							<th>Seller</th>
							<th>Price</th>
							<th>Live Tags</th>
							<th>All Tags</th>
							<th></th>
						</tr>
					</thead>
					<tbody>
						{#each state.auctionSocketMessages as message, index (index)}
							{#each message.LiveItems as liveItem, index2 (index2)}
								<tr class="{message.BuyItem.ID}-{index}" out:fade={{ duration: 100 }}>
									<td>{message.BuyItem.Name}</td>
									<td>{message.BuyItem.Price}</td>
									<td class="font-xx-small flex">
										{#each groupTags(message.BuyItem.AuctionTags) as tags, aIndex}
											<span class="overflow-x-auto">
												{tags[0]}:
												{#each tags.slice(1) as entries, eIndex} ({entries}) {/each}
											</span>
										{/each}
									</td>
									<td>{liveItem.auctioneer}</td>
									<td class="text-right">{Math.max(Number(liveItem.highest_bid_amount), Number(liveItem.starting_bid))}</td>
									<td >
										{#each groupTags(liveItem.AuctionTags.filter(e => message.BuyItem.AuctionTags.find(ee => ee.Name === e.Name))) as tags, aIndex}
											<span class="overflow-x-auto">
												{tags[0]}:
												{#each tags.slice(1) as entries, eIndex} ({entries}) {/each}
											</span>
										{/each}
									</td>
									<td class="expandable">
										<details>
											<summary>
												MORE
											</summary>
												{#each groupTags(liveItem.AuctionTags.filter(e => e.Value)) as tags, aIndex}
												<span class="overflow-x-auto">
												{tags[0]}:
												{#each tags.slice(1) as entries, eIndex} ({entries}) {/each}
											</span>
											{/each}
										</details>
									</td>
									<td class="px-1.5">
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