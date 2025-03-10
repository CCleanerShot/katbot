<script lang="ts">
	import { preloadData, pushState } from '$app/navigation';
	import { page } from '$app/state';
	import { pageState } from '$lib/states/pageState.svelte';
	import type { BasePageData } from '$lib/types';
	import type { Snippet } from 'svelte';
	import type { SvelteHTMLElements } from 'svelte/elements';

	type Props = {
		children: Snippet[];
		href: string;
		props?: SvelteHTMLElements['a'];
	};

	const { children, href, ...props } = $props();

	const onclick = async () => {
		pushState(href, page.data);

		const response = await preloadData(href);

		if (response.type === 'loaded') {
			pageState.page.data = response.data as BasePageData;
		}
	};
</script>

<a {...props} {href} {onclick}>
	{@render children()}
</a>
