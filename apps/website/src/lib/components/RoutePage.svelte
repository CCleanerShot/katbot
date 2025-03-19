<script lang="ts">
	import { afterNavigate } from '$app/navigation';
	import { page } from '$app/state';
	import type { ROUTES } from '$lib/other/routes';
	import Link from './Link.svelte';

	type Props = {
		route: (typeof ROUTES)['PAGES'][number];
		title: string;
		className?: string;
		imageUrl?: string;
	};

	const { route, title, className, imageUrl }: Props = $props();

	let style = $state('');

	afterNavigate(() => {
		style = page.route.id!.includes(route) ? 'background-color: var(--color-primary)' : '';
	});
</script>

<Link class={['button flex items-center justify-between gap-2', className]} href={route} {style}>
	{#if imageUrl}
		<img alt="related to the route" class="h-auto w-8" src={imageUrl} />
	{/if}
	<span class="m-auto">{title}</span>
</Link>
