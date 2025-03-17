<script lang="ts">
	import { onMount, type Snippet } from 'svelte';
	import { panelState } from '$lib/states/panelState.svelte';
	import type { SvelteHTMLElements } from 'svelte/elements';

	type Props = {
		children: Snippet<[]>;
		panel: keyof typeof panelState;
		title: Snippet<[SvelteHTMLElements['h3']]>;
		id?: string;
	};

	const { children, panel, title, id }: Props = $props();
	const current = panelState[panel];

	const onclick = () => {
		current.isOpened = false;
	};

	let invisible = $state(true);

	onMount(() => setTimeout(() => (invisible = false), 4000));
</script>

{#snippet close(className: string)}
	<div class={['relative', className]}>
		<span class="visible group-hover:invisible">▼ ▼ ▼</span>
		<span class="invisible absolute inset-0 transition group-hover:visible group-hover:rotate-3">CLOSE</span>
	</div>
{/snippet}

<div {id} class={['root-container', current.isOpened ? 'open' : '']}>
	<button class="header group flex border-b-2 p-1 font-bold" {onclick}>
		{@render close('')}
		{@render title({ class: 'flex-1' })}
		{@render close('')}
	</button>
	{@render children()}
</div>

<style>
	.root-container {
		background-color: white;
		border-top: 0.5rem solid black;
		bottom: -100%;
		display: flex;
		flex-direction: column;
		overflow-x: hidden;
		position: fixed;
		transition-property: bottom;
		transition-duration: var(--transition-time-long);
		width: 100vw;
		z-index: 30;
	}

	.root-container.open {
		bottom: 0px;
	}

	.header {
		cursor: pointer;
		padding-left: 0.4rem;
		padding-right: 0.4rem;
	}

	.header:hover {
		background-color: var(--primary-color);
		transition-duration: var(--transition-time-long);
		transition-property: background-color;
	}
</style>
