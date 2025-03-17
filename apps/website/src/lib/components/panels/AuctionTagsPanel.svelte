<script lang="ts">
	import Panel from './Panel.svelte';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import { auctionState } from '$lib/states/auctionState.svelte';
	import type { AuctionTag } from '$lib/mongodb/AuctionTag';
	import { TagType } from '$lib/enums';

	let item = $derived(auctionState.ITEM);

	const onclickAdd = () => {
		item.AuctionTags.push({ Name: '', Type: TagType.None, Value: '' });
	};

	const onclickEdit = (tag: AuctionTag) => {};
</script>

{#snippet title(props: SvelteHTMLElements['h3'])}
	<h3 class={['m-auto text-center ', props.class]}>
		<u>{item.Name}</u>
	</h3>
{/snippet}

<Panel panel="AuctionTagsPanel" {title}>
	<div class="flex h-64 flex-col p-2 pt-3">
		<div class="font-x-small-recursive flex-1 *:mx-1">
			{#each item.AuctionTags as tag}
				<button class="button" onclick={() => onclickEdit(tag)}>{tag.Name} ({tag.Value})</button>
			{/each}
		</div>
		<button class="button w-30 self-center" onclick={onclickAdd}>ADD</button>
	</div>
</Panel>
