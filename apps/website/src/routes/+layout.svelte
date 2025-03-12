<script lang="ts">
	import '../app.css';
	import Header from '$lib/components/Header.svelte';
	import { page } from '$app/state';
	import type { BasePageData } from '$lib/types';
	import { pageState } from '$lib/states/pageState.svelte';
	import { onMount } from 'svelte';

	let { children } = $props();

	onMount(() => {
		pageState.page.data = page.data as BasePageData;
	});

	let pageData = $derived(pageState.page.data);
</script>

<svelte:head>
	<title>{pageData.title}</title>
	<meta name="description" content={pageData.description} />
</svelte:head>

<main class="min-h-screen">
	<Header />
	{@render children()}
</main>
