<script lang="ts">
	import { page } from '$app/state';
	import RoutePage from '$lib/components/RoutePage.svelte';

	const { children } = $props();
</script>

<div>
	<div class="flex items-center justify-center gap-2 border-b-2 border-black py-2">
		{#if page.route.id?.includes('bazaar')}
			<span class="invisible rotate-3 whitespace-nowrap text-[6px] transition hover:rotate-2">
				check
				<a target="_blank" href="https://www.skyblock.bz" class="not-visited:text-blue-500 inline visited:text-purple-500 hover:underline">
					https://www.skyblock.bz
				</a>
				for prices.
			</span>
		{/if}
		<RoutePage className="w-24" route="/skyblock/auctions" title="Auctions" />
		<RoutePage className="w-24" route="/skyblock/bazaar" title="Bazaar" />
		{#if page.route.id?.includes('bazaar')}
			<span class="rotate-3 whitespace-nowrap text-[6px] transition hover:rotate-2">
				check
				<a target="_blank" href="https://www.skyblock.bz" class="not-visited:text-blue-500 inline visited:text-purple-500 hover:underline">
					https://www.skyblock.bz
				</a>
				for prices.
			</span>
		{/if}
	</div>
	<div>{@render children()}</div>
</div>
