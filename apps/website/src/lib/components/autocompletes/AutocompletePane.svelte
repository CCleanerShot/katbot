<script lang="ts">
	import type { ArrayType } from '$lib/types';
	import { autocompleteState } from '$lib/states/autocompleteState.svelte';

	let state = $derived(autocompleteState);

	const onclick = (input: ArrayType[number]) => {
		state.results = [];
		state.updateObj[state.updateKey] = input.Name;
		state.element!.value = input.Name;
		state.afterAction(input.Name);
	};

	const onfocusout = () => {
		state.results = [];
		state.element = undefined;
	};
</script>

{#if state.results.length > 0}
	<div class="font-x-small-recursive absolute bottom-0 left-0 z-50 flex w-full flex-1 items-center" {onfocusout}>
		<div
			class={[
				'grid max-h-32 min-h-32 flex-1 gap-[2px] overflow-y-auto border-4 border-black bg-white p-2',
				state.results.length > 16 ? 'grid-cols-4' : 'grid-cols-3'
			]}
		>
			{#each state.results as result}
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
