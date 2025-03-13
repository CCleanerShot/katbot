<script lang="ts">
	import { cacheState } from '$lib/states/cacheState.svelte';
	import type { ArrayType } from '$lib/types';
	import type { SvelteHTMLElements } from 'svelte/elements';
	import AutoComplete from './AutoComplete.svelte';
	import { getContext } from 'svelte';

	type Props = {
		containerProps?: SvelteHTMLElements['div'];
		inputProps?: SvelteHTMLElements['input'];
		resultsProps?: SvelteHTMLElements['div'];
	};

	const { containerProps, inputProps, resultsProps }: Props = $props();
	let allItems = $derived(cacheState['BAZAAR']);

	const autoCompleteInput = (input: string) => {
		const results = [] as ArrayType;

		if (input.trim() == '') {
			return results;
		}

		allItems.forEach((e, i) => {
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
</script>

<AutoComplete {autoCompleteInput} {containerProps} {inputProps} {resultsProps} />
