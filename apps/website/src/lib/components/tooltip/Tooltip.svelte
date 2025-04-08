<script lang="ts">
	import { onMount, type Snippet } from 'svelte';
	import { tooltipState } from '$lib/states/tooltipState.svelte';

	type Props = {
		children: Snippet<[]>;
		tooltip: keyof typeof tooltipState;
		className?: string;
	};

	let element: HTMLDivElement;
	let { children, tooltip, className }: Props = $props();
	let currentState = $derived(tooltipState[tooltip]);

	//TODO: make config (stored locally) for the closing of the tooltip 
	onMount(() => {
		const keypress = (ev: KeyboardEvent) => {
			if (ev.code !== 'ESCAPE') {
				return;
			}
			
			currentState.isOpened = false;
		};

		const mousemove = (ev: MouseEvent) => {
			const rect = element.getBoundingClientRect();
			element.style.top = `${Math.min(ev.pageY, document.body.clientHeight - rect.height)}px`;
			element.style.left = `${Math.min(ev.pageX, document.body.clientWidth - rect.width)}px`;
		};

		window.addEventListener('keypress', keypress);
		window.addEventListener('mousemove', mousemove);

		return () => {
			window.removeEventListener('keypress', keypress);
			window.removeEventListener('mousemove', mousemove);
		};
	});
</script>

<div
	bind:this={element}
	class={[!currentState.isOpened ? 'hidden' : '', 'font-small absolute flex flex-col rounded-sm border-2 bg-white p-0.5', className]}
>
	<span class="font-x-small-recursive border-b text-right italic"><strong>ESC</strong> => CLOSE</span>
	{@render children()}
</div>
