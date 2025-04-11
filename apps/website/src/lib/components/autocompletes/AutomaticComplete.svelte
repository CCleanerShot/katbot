<script lang="ts">
	import type { ArrayType } from '$lib/types';
	import type { SvelteHTMLElements } from 'svelte/elements';
	import { autocompleteState } from '$lib/states/autocompleteState.svelte';

	type Props = {
		array: { Name: string }[];
		updateKey: string;
		updateObj: Record<string, any>;
		value: string;
		afterAction?: (input: string) => void;
		inputProps?: SvelteHTMLElements['input'];
	};

	let {
		array = $bindable(),
		updateKey,
		updateObj = $bindable(),
		value = $bindable(),
		afterAction = (input) => ({}),
		inputProps
	}: Props = $props();
	let element: HTMLInputElement | undefined = $state();
	let pane = $derived(autocompleteState);

	type InputEvent = Event & {
		currentTarget: EventTarget & HTMLInputElement;
	};

	const autoCompleteInput = (input: string, array: { Name: string }[]) => {
		const results = [] as ArrayType;
		
		if (input.trim() == '') {
			return results;
		}
		
		array.forEach((e, i) => {
			const name = e.Name.toLowerCase();
			const match = name.match(input);
			
			if (match != null) {
				const min = match.index ?? 0;
				const max = min + input.length;
				results.push({ ...e, beg: e.Name.slice(0, min), mid: e.Name.slice(min, max), end: e.Name.slice(max) });
			}
		});
		
		return results;
	};

	const oninput = (e: InputEvent) => {
		if (inputProps?.oninput) {
			inputProps.oninput(e);
		}

		const text = e.currentTarget.value.toLowerCase();
		pane.afterAction = afterAction;
		pane.element = element;
		pane.results = autoCompleteInput(text, array);
		pane.updateKey = updateKey;
		pane.updateObj = updateObj;
	};
</script>

<input
	size="20"
	{...inputProps}
	bind:this={element}
	bind:value={
		() => value,
		(v) => {
			value = v;
			pane.initValue = v;
		}
	}
	{oninput}
/>
