<script lang="ts">
	import Link from './Link.svelte';
	import { page } from '$app/state';
	import { afterNavigate } from '$app/navigation';
	import type { ROUTES } from '$lib/other/routes';

	type Props = {
		route: (typeof ROUTES)['PAGES'][number];
		title: string;
		className?: string;
		classNameText?: string;
		imageUrl?: string;
		thin?: boolean;
	};

	const { route, title, className, classNameText, imageUrl, thin = false }: Props = $props();

	let style = $state('');

	afterNavigate(() => {
		style = page.route.id!.includes(route) ? 'background-color: var(--color-primary)' : '';
	});
</script>

<Link class={['flex items-center justify-between gap-2', thin ? "button button-thin" : "button", className]} href={route} {style}>
	{#if imageUrl}
		<img alt="related to the route" class="h-auto w-8" src={imageUrl} />
	{/if}
	<span class={['m-auto', classNameText]}>{title}</span>
</Link>
