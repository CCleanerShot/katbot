<script lang="ts">
	import RoutePage from '$lib/components/RoutePage.svelte';

	const { children } = $props();
</script>

<div>
	<div class=" flex justify-center gap-2 border-b-2 border-black py-2 *:w-20">
		<RoutePage route="/skyblock/auctions" title="Auctions" />
		<RoutePage route="/skyblock/bazaar" title="Bazaar" />
	</div>
	<div>{@render children()}</div>
</div>
