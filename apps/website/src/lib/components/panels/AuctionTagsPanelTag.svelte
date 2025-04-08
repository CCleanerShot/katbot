<script lang="ts">
	import type { AuctionTag } from '$lib/mongodb/AuctionTag';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { panelState } from '$lib/states/panelState.svelte';
	import type { SvelteHTMLElements } from 'svelte/elements';
	import AutoComplete from '../autocompletes/Autocomplete.svelte';
	import { TagType } from '$lib/enums';

	type Props = {
		tag: AuctionTag;
		props?: SvelteHTMLElements['div'];
	};

	let { tag = $bindable(), props }: Props = $props();
	let panel = $derived(panelState.AuctionTagsPanel);
	let names = $derived(cacheState['AUCTIONS'].items.find((e) => e.ID === panel.item.ID)?.AuctionTags.map((e) => ({ Name: e })) ?? []);
	let values = $derived(cacheState['AUCTIONS'].tags.find((e) => e.Name === tag.Name)?.Values.map((e) => ({ Name: e })) ?? []);
	let selected = $derived(panel.tag === tag);

	const onfocusinEdit = (tag: AuctionTag) => {
		panel.tag = tag;
	};

	const onfocusout = (e: FocusEvent & { currentTarget: EventTarget & HTMLDivElement }) => {
		if (props?.onfocusout) {
			props?.onfocusout(e);
		}

		panel.tag = undefined;
	};

	const onclickDelete = (e: Event) => {
		const index = panel.item.AuctionTags.findIndex((e) => e === tag);

		if (index !== -1) {
			panel.item.AuctionTags.splice(index, 1);
		}
	};

	const afterAction = (input: string) => {
		tag.Type = cacheState.AUCTIONS.tags.find((e) => e.Name === input)?.Type ?? TagType.None;
	};
</script>

<div {...props} class={['button flex w-64', { selected }, props?.class]} onfocusincapture={() => onfocusinEdit(tag)} {onfocusout}>
	<AutoComplete
		{afterAction}
		array={names}
		bind:updateObj={tag}
		bind:value={tag.Name}
		inputProps={{ class: 'input flex-[10]', size: 10 }}
		updateKey={'Name'}
	/>
	<AutoComplete
		array={values}
		bind:updateObj={tag}
		bind:value={tag.Value}
		inputProps={{ class: 'input flex-[10]', size: 10 }}
		updateKey={'Value'}
	/>
	<button onclick={onclickDelete}>
		{#if selected}
			X
		{/if}
	</button>
</div>

<style>
	button {
		background-color: var(--color-red);
		border: 2px solid black;
		padding-left: 0.2rem;
		padding-right: 0.2rem;
		margin-left: var(--button-padding-x);
		flex: 1;
	}

	button:hover {
		scale: 105%;
	}

	div.selected {
		background-color: var(--color-primary);
	}

	div:not(.selected) button {
		border: none;
		padding-left: 0px;
		padding-right: 0px;
		margin-left: 0px;
		flex: 0;
		width: 0px;
	}
</style>
