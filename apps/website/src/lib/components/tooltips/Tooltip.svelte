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
	let isFrozen = $state(false);

	const keypress = (ev: KeyboardEvent) => {
		if (ev.code !== 'KeyC' && ev.code !== 'KeyF') {
			return;
		}

		switch (ev.code) {
			case 'KeyC':
				currentState.isOpened = false;
				break;
			case 'KeyF':
				isFrozen = !isFrozen;
				break;
		}
	};

	const mousemove = (ev: MouseEvent) => {
		if (isFrozen) {
			return;
		}

		const rect = element.getBoundingClientRect();
		element.style.top = `${Math.min(ev.pageY + 8, document.body.clientHeight - rect.height)}px`;
		element.style.left = `${Math.min(ev.pageX + 8, document.body.clientWidth - rect.width)}px`;
	};

	onMount(() => {
		console.log('a');

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
	class={[!currentState.isOpened ? 'hidden' : '', 'font-small absolute z-50 flex flex-col rounded-sm border-2 bg-white p-0.5', className]}
>
	<div class="flex justify-around">
		<span class="font-x-small-recursive border-b text-center italic"><strong>C</strong> => CLOSE</span>
		<span class="font-x-small-recursive border-b text-center italic"><strong>F</strong> => FREEZE</span>
	</div>
	{@render children()}
</div>
