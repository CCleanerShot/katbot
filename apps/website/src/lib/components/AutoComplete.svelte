<script lang="ts">
	import { cacheState } from '$lib/states/cacheState.svelte';
	import type { ItemType } from '$lib/types';
	import type { SvelteHTMLElements } from 'svelte/elements';

	type Props = {
		type: ItemType & {}; // TODO: add others if needed
		containerProps?: SvelteHTMLElements['div'];
		inputProps?: SvelteHTMLElements['input'];
		resultsProps?: SvelteHTMLElements['div'];
	};

	type InputEvent = Event & {
		currentTarget: EventTarget & HTMLInputElement;
	};

	const { type, containerProps, inputProps, resultsProps }: Props = $props();

	let autocompleteQuery = $derived(cacheState[type]);
	const oninput = (e: InputEvent) => {
		if (inputProps?.oninput) {
			inputProps.oninput(e);
		}
	};

	$inspect(autocompleteQuery);
</script>

<div {...containerProps} class={['relative', containerProps?.class]}>
	<input {...inputProps} {oninput} />
	<div {...resultsProps} class={['absolute', resultsProps?.class]}></div>
</div>
