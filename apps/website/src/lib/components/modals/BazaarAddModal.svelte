<script lang="ts">
	import { onMount } from 'svelte';
	import Modal from './Modal.svelte';
	import { Form } from '$lib/classes/Form.svelte';
	import { clientFetch } from '$lib/other/clientFetch';
	import { BazaarItem } from '$lib/mongodb/BazaarItem';
	import { API_CONTRACTS } from '$lib/other/apiContracts';
	import { modalState } from '$lib/states/modalState.svelte';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { FormElement } from '$lib/classes/FormElement.svelte';
	import BazaarItemsAllAutoComplete from '$lib/components/autocompletes/BazaarItemsAllAutoComplete.svelte';

	const { BazaarAddModal } = modalState;
	let items = $derived(cacheState.BAZAAR);
	let action = $derived(BazaarAddModal.action);
	let { params, route } = $derived(API_CONTRACTS[action]);
	let id: BazaarItem['ID'] | undefined = $derived(items.find((e) => e.Name === name)?.ID);
	let name: BazaarItem['Name'] = $state('');
	let price: BazaarItem['Price'] = $state(0n);
	let orderType: 0 | 1 = $state(1);
	let removeAfter: true | false = $state(true);

	const form = new Form([
		new FormElement('name', () => id !== undefined, 'not a valid name!'),
		new FormElement('price', () => true, 'how did we get here?'),
		new FormElement('order-type', () => orderType === 0 || orderType === 1, 'illegal input!'),
		new FormElement('remove-after', () => typeof removeAfter === 'boolean', 'illegal input!')
	]);

	onMount(() => {
		form.Mount();

		return () => {
			form.Clean();
		};
	});

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();

		if (!form.IsValid()) {
			return;
		}

		const newItem: typeof params = {
			item: {
				ID: id!,
				Name: name,
				OrderType: orderType,
				Price: price,
				RemovedAfter: removeAfter
			}
		};

		await clientFetch(action, newItem, true);
		await fetch(route, { body: JSON.stringify(newItem), method: 'POST' });
	};
</script>

<Modal modal="BazaarAddModal" id="modal">
	<form method="POST" class="flex flex-1 flex-col items-center justify-center gap-1 p-2" {onsubmit}>
		<div id="container" class="flex flex-1 flex-col gap-1 border border-black p-3">
			<div>
				<label for="name">Name</label>
				<BazaarItemsAllAutoComplete
					bind:value={name}
					inputProps={{ autocomplete: 'off', class: 'input', id: 'name', name: 'name', required: true }}
				/>
			</div>
			<div>
				<label for="price">Price</label>
				<input id="price" name="price" type="number" class="input" bind:value={price} required min="0" max="1000000000000" />
			</div>
			<div>
				<label for="order-type" class="text-[10px]">Order Type</label>
				<select id="order-type" name="order-type" class="input cursor-pointer" bind:value={orderType} required>
					<option value={0}>INSTA</option>
					<option value={1}>ORDER</option>
				</select>
			</div>
			<div>
				<label for="remove-after" class="text-[10px]">Remove After?</label>
				<select id="remove-after" name="remove-after" class="input hover: cursor-pointer" bind:value={removeAfter} required>
					<option value={true}>TRUE</option>
					<option value={false}>FALSE</option>
				</select>
			</div>
		</div>
		<button class="button-border mt-2 bg-green-500 p-1 px-2" type="submit">Create</button>
	</form>
</Modal>

<style>
	#container > div {
		display: flex;
		align-items: center;
	}

	#container label:hover {
		font-weight: 600;
		text-decoration: underline;
	}

	#container > div > label {
		width: 50%;
		cursor: pointer;
	}
</style>
