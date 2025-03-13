<script lang="ts">
	import type { ArrayType } from '$lib/types';
	import { setContext } from 'svelte';
	import type { SvelteHTMLElements } from 'svelte/elements';

	type Props = {
		autoCompleteInput: (input: string) => ArrayType;
		containerProps?: SvelteHTMLElements['div'];
		inputProps?: SvelteHTMLElements['input'];
		resultsProps?: SvelteHTMLElements['div'];
	};

	type InputEvent = Event & {
		currentTarget: EventTarget & HTMLInputElement;
	};

	const { autoCompleteInput, containerProps, inputProps, resultsProps }: Props = $props();
	let autoCompleteResults = $state([] as ArrayType);
	let value = $state('');

	const onclick = (input: ArrayType[number]) => {
		value = input.Name;
		autoCompleteResults = [];
	};

	const oninput = (e: InputEvent) => {
		if (inputProps?.oninput) {
			inputProps.oninput(e);
		}

		const text = e.currentTarget.value.toLowerCase();
		autoCompleteResults = autoCompleteInput(text);
	};
</script>

<div {...containerProps} class={['', containerProps?.class]}>
	<input {...inputProps} {oninput} bind:value />
	{#if autoCompleteResults.length > 0}
		<div class="absolute -bottom-0 -left-0 flex w-full flex-1 items-center">
			<div
				{...resultsProps}
				class={[
					'grid max-h-32 min-h-32 flex-1 gap-[2px] overflow-y-auto border-2 border-black bg-white p-1',
					resultsProps?.class,
					autoCompleteResults.length > 16 ? 'grid-cols-2' : 'grid-cols-1'
				]}
			>
				{#each autoCompleteResults as result}
					<button
						type="button"
						class="cursor-pointer overflow-x-auto whitespace-nowrap border border-gray-500 px-1 text-[7px] transition hover:bg-green-600"
						onclick={() => onclick(result)}
					>
						<span>{result.beg}</span><span class="font-bold text-green-500">{result.mid}</span><span>{result.end}</span>
					</button>
				{/each}
			</div>
		</div>
	{/if}
</div>
