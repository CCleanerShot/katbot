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

{#snippet close()}
	<button {onclick} class="down-arrow-button group relative">
		<span class="visible group-hover:invisible">▼ ▼ ▼</span>
		<span class="invisible absolute inset-0 transition-colors group-hover:visible group-hover:rotate-3">CLOSE</span>
	</button>
{/snippet}

<div {id} class={['root-container', current.isOpened ? 'open' : 'hide']}>
	<div class="flex border-b-2 p-1 font-bold">
		{@render close()}
		{@render title({ class: 'flex-1' })}
		{@render close()}
	</div>
	{@render children()}
</div>

<style>
	@keyframes slide-in {
		100% {
			opacity: 100%;
			transform: translateY(0%);
		}
	}

	@keyframes slide-out {
		100% {
			opacity: 0%;
			transform: translateY(100%);
		}
	}

	.root-container {
		animation-duration: 0.4s;
		animation-iteration-count: 1;
		background-color: white;
		bottom: 0px;
		display: flex;
		flex-direction: column;
		opacity: 0%;
		overflow-x: hidden;
		position: fixed;
		transform: translateY(100%);
		width: 100vw;
	}

	.root-container.hide {
		animation-fill-mode: forwards;
		animation-name: slide-out;
	}

	.root-container.open {
		animation-fill-mode: forwards;
		animation-name: slide-in;
	}

	.down-arrow-button {
		border: 1px solid black;
		cursor: pointer;
		padding-left: 0.4rem;
		padding-right: 0.4rem;
	}

	.down-arrow-button:hover {
		background-color: var(--primary-color);
		transition-duration: 0.4s;
		transition-property: background-color;
	}
</style>
