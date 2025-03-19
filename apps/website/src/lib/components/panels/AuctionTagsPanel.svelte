<script lang="ts">
	import Panel from './Panel.svelte';
	import { type SvelteHTMLElements } from 'svelte/elements';
	import type { AuctionTag } from '$lib/mongodb/AuctionTag';
	import { TagType } from '$lib/enums';
	import AuctionTagsPanelTag from './AuctionTagsPanelTag.svelte';
	import { panelState } from '$lib/states/panelState.svelte';

	let item = $derived(panelState.AuctionTagsPanel?.item!);
	let tags = $derived(item?.AuctionTags);
	let pane = $derived(panelState.AuctionTagsPanel);

	const onclickAdd = () => {
		const tag: AuctionTag = { Name: '', Type: TagType.None, Value: '' };
		item.AuctionTags.push(tag);
		pane.tag = tag;
	};
</script>

{#snippet title(props: SvelteHTMLElements['h3'])}
	<h3 class={['m-auto text-center ', props.class]}>
		<u>{item.Name}</u>
	</h3>
{/snippet}

<Panel panel="AuctionTagsPanel" {title}>
	<div class="flex h-64 flex-col p-2 pt-3">
		<div class="font-x-small-recursive flex flex-wrap items-start justify-start gap-2 *:min-w-32">
			{#each tags as tag, index (index)}
				<AuctionTagsPanelTag bind:tag={tags[index]} />
			{/each}
		</div>
		<button id="add" class="button w-30 mt-2 self-center" onclick={onclickAdd}>ADD</button>
	</div>
</Panel>

<style>
	#add {
		background-color: var(--color-green);
	}
</style>
