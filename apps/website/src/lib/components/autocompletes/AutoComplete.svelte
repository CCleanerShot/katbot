<script lang="ts">
	import type { ArrayType } from '$lib/types';
	import type { SvelteHTMLElements } from 'svelte/elements';

	type Props = {
		autoCompleteInput: (input: string) => ArrayType;
		value: string;
		containerProps?: SvelteHTMLElements['div'];
		inputProps?: SvelteHTMLElements['input'];
		resultsProps?: SvelteHTMLElements['div'];
	};

	type InputEvent = Event & {
		currentTarget: EventTarget & HTMLInputElement;
	};

	let { autoCompleteInput, value = $bindable(), containerProps, inputProps, resultsProps }: Props = $props();
	let autoCompleteResults = $state([] as ArrayType);

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

<div {...containerProps} class={['relative flex', containerProps?.class]}>
	<input {...inputProps} {oninput} bind:value class={['flex-1', inputProps?.class]} />
	{#if autoCompleteResults.length > 0}
		<div class="font-x-small-recursive absolute left-0 top-[100%] z-10 flex w-full flex-1 items-center">
			<div
				{...resultsProps}
				class={[
					'grid max-h-64 min-h-64 flex-1 gap-[2px] overflow-y-auto border-4 border-black bg-white p-2',
					resultsProps?.class,
					autoCompleteResults.length > 16 ? 'grid-cols-2' : 'grid-cols-1'
				]}
			>
				{#each autoCompleteResults as result}
					<button
						type="button"
						class="overflow-x-auto whitespace-nowrap border border-gray-500 px-1 transition hover:bg-green-600"
						onclick={() => onclick(result)}
					>
						<span>{result.beg}</span><span class="font-bold text-green-500">{result.mid}</span><span>{result.end}</span>
					</button>
				{/each}
			</div>
		</div>
	{/if}
</div>
