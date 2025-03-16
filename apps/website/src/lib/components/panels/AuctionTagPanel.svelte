<script lang="ts">
	import Panel from './Panel.svelte';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import { auctionState } from '$lib/states/auctionState.svelte';

	let item = $derived(auctionState.ITEM);
</script>

{#snippet title(props: SvelteHTMLElements['h3'])}
	<h3 class={['text-center', props.class]}>
		EDIT TAGS FOR: <button class="title-button"><u>{item.Name}</u></button>
	</h3>
{/snippet}

<Panel panel="AuctionTagPanel" {title}>
	<div class="h-24 p-1 *:mx-1">
		{#each item.AuctionTags as tag}
			<button class="button">{tag.Name} ({tag.Value})</button>
		{/each}
	</div>
</Panel>

<style>
	.title-button {
		cursor: pointer;
	}

	.title-button:hover {
		color: var(--secondary-color);
	}
</style>
