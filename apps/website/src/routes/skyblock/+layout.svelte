<script lang="ts">
	import { page } from '$app/state';
	import RoutePage from '$lib/components/RoutePage.svelte';

	const { children } = $props();
</script>

{#snippet link(className: string)}
	<span class={['font-x-small rotate-3 whitespace-nowrap transition hover:rotate-2', className]}>
		check
		<a
			target="_blank"
			href="https://www.skyblock.bz"
			class="font-x-small not-visited:text-blue-500 visited:text-purple-500 hover:underline"
		>
			https://www.skyblock.bz
		</a>
		for prices.
	</span>
{/snippet}
<div>
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
	<div>{@render children()}</div>
</div>
