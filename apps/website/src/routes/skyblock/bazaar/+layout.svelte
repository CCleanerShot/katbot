<script lang="ts">
	import { onMount } from 'svelte';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';

	const { children } = $props();

	onMount(async () => {
		const response = await utilityClient.fetch('GET=>/api/bazaar', {});
		cacheState.BAZAAR = (await response.JSON()).data;
	});
</script>

{@render children()}
