<script lang="ts">
	import '../polyfills';
	import '../app.css';
	import Header from '$lib/components/Header.svelte';
	import { page } from '$app/state';
	import type { BasePageData } from '$lib/types';
	import { pageState } from '$lib/states/pageState.svelte';
	import { onMount } from 'svelte';
	import Toasts from '$lib/components/toast/Toasts.svelte';

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
	<Header />
	<Toasts />
	{@render children()}
</main>
