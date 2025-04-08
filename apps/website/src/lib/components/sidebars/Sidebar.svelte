<script lang="ts">
	import { onMount, type Snippet } from 'svelte';
	import type { SvelteHTMLElements } from 'svelte/elements';
	import { sidebarState } from '$lib/states/sidebarState.svelte';

	type Props = {
		children: Snippet<[]>;
		sidebar: keyof typeof sidebarState;
		title: Snippet<[SvelteHTMLElements['h3']]>;
	};

	const { children, sidebar, title }: Props = $props();
	const current = sidebarState[sidebar];
	let invisible = $state(true);
	let dragging = $state(false);
	let lastX = $state(0);
	let sidebarElement: HTMLDivElement;

	onMount(() => setTimeout(() => (invisible = false), 4000));

	const ondrag = (e: DragEvent & { currentTarget: EventTarget & HTMLDivElement }) => {
		// the last e.clientX is 0 when releasing. this is created to prevent 1frame snapback
		if (lastX >= 30 && e.clientX === 0) {
			return;
		}

		lastX = e.clientX;
		sidebarElement.style.width = e.clientX.toString() + 'px';
	};

	const ondragend = (e: DragEvent & { currentTarget: EventTarget & HTMLDivElement }) => {
		dragging = false;
		sidebarElement.style.width = e.clientX.toString() + 'px';
	};

	const ondragstart = (e: DragEvent & { currentTarget: EventTarget & HTMLDivElement }) => {
		dragging = true;
	};
</script>

{#snippet close(className: string)}
	<div class={['relative flex flex-col justify-evenly', className]}>
		{#each new Array(6).fill(null) as item, index}
			<span class="visible">◄</span>
		{/each}
	</div>
{/snippet}

<div>
	<button
		class={['absolute-y-center widget open-sidebar ml-2 flex flex-col items-center justify-center', !current.isOpened ? 'open' : '']}
		onclick={() => (current.isOpened = true)}
	>
		{#each new Array(3).fill(null) as item, index}
			<span class="visible">►</span>
		{/each}
	</button>
	<div bind:this={sidebarElement} class={['bg-white widget sidebar flex w-[20rem] md:w-[30rem]', current.isOpened ? 'open' : '']}>
		<div class="flex flex-1 flex-col bg-white">
			{@render title({ class: 'border-b-2' })}
			{@render children()}
		</div>
			<div class="flex bg-white">
				<button class="header group flex border-l-2 p-1 font-bold" onclick={() => (current.isOpened = false)}>
					{@render close('')}
				</button>
				<div
				class={['pullable', dragging ? 'dragging' : 'dragging cursor-grab']}
				draggable="true"
				{ondrag}
				{ondragend}
				{ondragstart}
				role="application"
				></div>
			</div>
	</div>
</div>

<style>
	.widget {
		display: flex;
		left: -100%;
		position: fixed;
		transition-property: left, background-color;
		transition-duration: var(--transition-time-medium);
		z-index: 30;
	}

	.header {
		cursor: pointer;
		padding-left: 0.4rem;
		padding-right: 0.4rem;
		transition-property: background-color;
		transition-duration: var(--transition-time-medium);
	}

	.header:hover {
		background-color: var(--color-primary);
	}

	.open-sidebar {
		padding: 0.5rem;
	}

	.sidebar {
		height: 100vh;
		top: 0px;
	}

	.open {
		left: 0px;
	}

	.pullable {
		background-color: black;
		padding: 0.25rem;
		transition-property: color, background-color;
		transition-duration: var(--transition-time-medium);
	}

	.pullable.dragged {
		background-color: var(--color-secondary);
	}

	.pullable:hover {
		background-color: var(--color-secondary);
	}
</style>
