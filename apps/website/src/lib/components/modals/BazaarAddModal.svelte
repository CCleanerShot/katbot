<script lang="ts">
	import { API_CONTRACTS } from '$lib/other/apiContracts';
	import { modalState } from '$lib/states/modalState.svelte';
	import AutoComplete from '../AutoComplete.svelte';
	import Modal from './Modal.svelte';

	const { BazaarAddModal } = modalState;

	let action = $derived(
		BazaarAddModal.type === 'BUYS'
			? API_CONTRACTS['POST=>/api/bazaar/buy']['route']
			: API_CONTRACTS['POST=>/api/bazaar/sell']['route']
	);
</script>

<Modal modal="BazaarAddModal">
	<form method="POST" {action} class="flex flex-1 flex-col items-center justify-center gap-1 p-1">
		<div id="container" class=" flex flex-col gap-1 border border-black p-3">
			<div>
				<label for="name">Name</label>
				<AutoComplete type="BAZAAR" inputProps={{ id: 'name', class: 'input' }} />
			</div>
			<div>
				<label for="price">Price</label>
				<input id="price" type="number" class="input" />
			</div>
			<div>
				<label for="order-type" class="text-[10px]">Order Type</label>
				<input id="order-type" type="number" class="input" />
			</div>
			<div>
				<label for="remove-after" class="text-[10px]">Remove After?</label>
				<input id="remove-after" type="number" class="input" />
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
		width: 4.5rem;
		cursor: pointer;
	}
</style>
