<script lang="ts">
	import { toastsState } from '$lib/states/toastsState.svelte';
	import type { ToastProps } from '$lib/types';
	import { onMount } from 'svelte';
	import { fly } from 'svelte/transition';

	let toasts = $derived(toastsState);
	let { id, message, type }: ToastProps = $props();
	let timer: NodeJS.Timeout | undefined = $state();

	const remove = () => {
		const index = toasts.findIndex((e) => e.id === id);
		toasts.splice(index, 1);
	};

	const reset = () => {
		clearTimeout(timer);
	};

	const setTimer = () => {
		timer = setTimeout(remove, 5000 + toasts.length * 500);
	};

	onMount(() => {
		setTimer();
		return reset;
	});

	const color = type === 'ERROR' ? 'red' : type === 'NONE' ? 'green' : 'yellow';
</script>

<button onclick={remove} transition:fly={{ y: -100 }} onmouseenter={reset} onmouseleave={setTimer} class={['toast', color]}>
	<span class="flex-1 p-0.5">{message}</span>
</button>

<style>
	.toast {
		background-color: white;
		border-left: 0.25rem solid;
		border-right: 0.25rem solid;
		border-radius: 0.5rem;
		display: flex;
		font-size: 0.5rem;
		transition-property: all;
		transition-duration: var(--transition-time-medium);
	}

	.toast:hover {
		background-color: black;
		color: white;
		padding-left: 0.2rem;
		padding-right: 0.2rem;
		border-left-width: 1rem;
		border-right-width: 1rem;
		transform: scale(105%);
	}

	.green {
		border-color: green;
	}

	.yellow {
		border-color: yellow;
	}

	.red {
		border-color: red;
	}
</style>
