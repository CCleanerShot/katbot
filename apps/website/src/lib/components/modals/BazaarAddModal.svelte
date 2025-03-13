<script lang="ts">
	import Modal from './Modal.svelte';
	import { API_CONTRACTS } from '$lib/other/apiContracts';
	import { modalState } from '$lib/states/modalState.svelte';
	import BazaarItemsAllAutoComplete from '../autocompletes/BazaarItemsAllAutoComplete.svelte';
	import { page } from '$app/state';

	const { BazaarAddModal } = modalState;

	let element: HTMLFormElement;
	let action = $derived(BazaarAddModal.type === 'BUYS' ? API_CONTRACTS['FORM=>/?create/buy'] : API_CONTRACTS['FORM=>/?create/sell']);

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();

		const response = await fetch(action['route'], {
			body: new FormData(element),
			method: 'POST'
		});

		console.log(response);
		return e;
	};
</script>

<Modal modal="BazaarAddModal">
	<form bind:this={element} method="POST" class="flex flex-1 flex-col items-center justify-center gap-1 p-2" {onsubmit}>
		<div id="container" class="flex flex-1 flex-col gap-1 border border-black p-3">
			<div>
				<label for="name">Name</label>
				<BazaarItemsAllAutoComplete inputProps={{ class: 'input', name: 'name' }} />
			</div>
			<div>
				<label for="price">Price</label>
				<input id="price" name="price" type="number" class="input" />
			</div>
			<div>
				<label for="order-type" class="text-[10px]">Order Type</label>
				<select id="order-type" name="order-type" class="input" value="order">
					<option value="insta">INSTA</option>
					<option value="order">ORDER</option>
				</select>
			</div>
			<div>
				<label for="remove-after" class="text-[10px]">Remove After?</label>
				<select id="remove-after" name="remove-after" class="input" value="true">
					<option value="true">TRUE</option>
					<option value="false">FALSE</option>
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
