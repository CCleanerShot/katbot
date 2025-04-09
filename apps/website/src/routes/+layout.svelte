<script lang="ts">
	import '../app.css';
	import '../polyfills';
	import '../app-font.css';
	import '../app-media.css';
	import '../app-special.css';
	import '../app-animations.css';
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import type { BasePageData } from '$lib/types';
	import Header from '$lib/components/Header.svelte';
	import { pageState } from '$lib/states/pageState.svelte';
	import Toasts from '$lib/components/toasts/Toasts.svelte';
	import Tooltip from '$lib/components/tooltips/Tooltip.svelte';
	import LoadingBorder from '$lib/components/LoadingBorder.svelte';
	import AutoCompletePane from '$lib/components/autocompletes/AutocompletePane.svelte';

	let { children } = $props();
	let pageData = $derived(pageState.page.data);

	onMount(() => {
		pageState.page.data = page.data as BasePageData;
	});
</script>

<svelte:head>
	<title>{pageData.title}</title>
	<meta name="description" content={pageData.description} />
</svelte:head>

<main class="min-h-screen">
	<LoadingBorder />
	<Header />
	<Toasts />
	<AutoCompletePane />
	{@render children()}
</main>
