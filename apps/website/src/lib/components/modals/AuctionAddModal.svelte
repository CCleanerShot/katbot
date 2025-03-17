<script lang="ts">
	import { onMount } from 'svelte';
	import Modal from './Modal.svelte';
	import { Form } from '$lib/classes/Form.svelte';
	import { API_CONTRACTS } from '$lib/other/apiContracts';
	import { modalState } from '$lib/states/modalState.svelte';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { FormElement } from '$lib/classes/FormElement.svelte';
	import AuctionItemsAllAutoComplete from '../autocompletes/AuctionItemsAllAutoComplete.svelte';
	import type { AuctionItem } from '$lib/mongodb/AuctionItem';
	import { auctionState } from '$lib/states/auctionState.svelte';
	import { panelState } from '$lib/states/panelState.svelte';

	class AuctionModalItem {
		AuctionTags: AuctionItem['AuctionTags'] = $state([]);
		ID: AuctionItem['ID'] | undefined = $derived(items.find((e) => e.Name === this.Name)?.ID);
		Name: AuctionItem['Name'] = $state('');
		Price: AuctionItem['Price'] = $state(0n);
		RemovedAfter: AuctionItem['RemovedAfter'] = $state(true);
		UserId: AuctionItem['UserId'] = 0n; // is set during submit; should not assume from buys because there may not be any.
	}

	const { AuctionAddModal } = modalState;
	const item = new AuctionModalItem();
	let items = $derived(cacheState.AUCTIONS.items);
	let tags = $derived(cacheState.AUCTIONS.tags);
	let action = $derived(AuctionAddModal.action);
	let idExists = $derived(item.ID === undefined);
	let { params, route } = $derived(API_CONTRACTS[action]);

	const form = new Form([
		new FormElement('name', () => item.ID !== undefined, 'not a valid name!'),
		new FormElement('price', () => true, 'how did we get here?'),
		new FormElement('remove-after', () => typeof item.RemovedAfter === 'boolean', 'illegal input!')
	]);

	onMount(() => {
		form.Mount();

		return () => {
			form.Clean();
		};
	});

	const onclick = () => {
		auctionState.ITEM = item;
		panelState.AuctionTagsPanel.isOpened = true;
	};

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();

		if (!form.IsValid()) {
			return;
		}

		// await clientFetch(action, newItem, true);
		// await fetch(route, { body: JSON.stringify(newItem), method: 'POST' });
	};
</script>

<Modal modal="AuctionAddModal" id="modal" title="ADD AN AUCTION ITEM">
	<form method="POST" class="flex flex-1 flex-col justify-center gap-2 p-2" {onsubmit}>
		<div class="form-container flex flex-1 flex-col gap-2 border border-black p-3">
			<div>
				<label for="name">Name</label>
				<AuctionItemsAllAutoComplete
					bind:value={item.Name}
					inputProps={{ autocomplete: 'off', class: 'input', id: 'name', name: 'name', required: true }}
				/>
			</div>
			<div>
				<label for="price">Price</label>
				<input id="price" name="price" type="number" class="input" bind:value={item.Price} required min="0" max="1000000000000" />
			</div>
			<div>
				<label for="remove-after" class="">Remove After?</label>
				<select id="remove-after" name="remove-after" class="input" bind:value={item.RemovedAfter} required>
					<option value={true}>TRUE</option>
					<option value={false}>FALSE</option>
				</select>
			</div>
			<div>
				<label for="tags">Tags</label>
				<button class={[idExists ? 'group relative bg-red-500' : 'bg-green-500']} disabled={idExists} id="edit-tag" {onclick} type="button">
					<span class={[idExists ? 'group-hover:invisible' : '']}>EDIT</span>
					{#if idExists}
						<span class="invisible absolute inset-0 transition group-hover:visible group-hover:rotate-2">ADD A NAME FIRST</span>
					{/if}
				</button>
			</div>
		</div>
		<button class="button-border my-3 self-center bg-green-500 p-1 px-2" type="submit">Create</button>
	</form>
</Modal>

<style>
	#edit-tag {
		border: 2px solid black;
		transition-property: background-color;
		transition-duration: var(--transition-time-short);
	}
</style>
