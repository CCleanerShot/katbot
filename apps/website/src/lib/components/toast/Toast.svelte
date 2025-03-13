<script lang="ts">
	import { toastsState } from '$lib/states/toastsState.svelte';
	import type { ToastProps } from '$lib/types';
	import { onMount } from 'svelte';

	let toasts = $state(toastsState);
	let { id, message, type }: ToastProps = $props();

	const remove = () => {
		const index = toasts.findIndex((e) => e.id === id);
		console.log(index, toasts);
		toasts.splice(index, 1);
	};

	onMount(() => {
		setTimeout(() => remove, 2000);
	});

	const onclick = () => remove;

	let element: HTMLElement | undefined = $state();

	const color = type === 'ERROR';
</script>

<div bind:this={element} class={['button flex', type === 'ERROR' ? 'bg-red-500' : type === 'NONE' ? 'bg-green-500' : 'bg-yellow-500']}>
	<span class="flex-1">{message}</span>
	<button class="border-l-2 border-black" {onclick}>CLOSE</button>
</div>
