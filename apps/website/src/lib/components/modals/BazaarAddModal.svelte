<script lang="ts">
	import Modal from './Modal.svelte';
	import { API_CONTRACTS } from '$lib/other/apiContracts';
	import { modalState } from '$lib/states/modalState.svelte';
	import BazaarItemsAllAutoComplete from '../autocompletes/BazaarItemsAllAutoComplete.svelte';
	import type { BazaarItem } from '$lib/mongodb/BazaarItem';
	import { cacheState } from '$lib/states/cacheState.svelte';
	import { onMount, type SvelteComponent } from 'svelte';
	import { toastsState } from '$lib/states/toastsState.svelte';
	import { utility } from '$lib/utility/utility';

	const { BazaarAddModal } = modalState;
	let items = $derived(cacheState.BAZAAR);
	let action = $derived(BazaarAddModal.type === 'BUYS' ? API_CONTRACTS['POST=>/api/bazaar/buy'] : API_CONTRACTS['POST=>/api/bazaar/sell']);
	let id: (typeof BazaarItem)['prototype']['ID'] | undefined = $derived(items.find((e) => e.Name === name)?.ID);
	let name: (typeof BazaarItem)['prototype']['Name'] = $state('');
	let price: (typeof BazaarItem)['prototype']['Price'] = $state(0n);
	let orderType: 0 | 1 = $state(1);
	let removeAfter: true | false = $state(true);

	class ItemElement<T extends HTMLInputElement = HTMLInputElement> {
		public isValid = $state(false);
		public element: T;
		public id: string;
		validation: () => boolean;
		invalidMessage: string;

		static elements = [] as ItemElement[];

		constructor(id: string, validation: () => boolean, invalidMessage: string) {
			this.id = id;
			this.element = undefined as any as T; // gets defined during onMount;
			this.validation = validation;
			this.invalidMessage = invalidMessage;

			ItemElement.elements.push(this);
		}

		Validate(): boolean {
			if (this.validation()) {
				return true;
			} else {
				this.element.setCustomValidity(this.invalidMessage);
				this.element.reportValidity();
				return false;
			}
		}
	}

	const nameElement = new ItemElement('name', () => id !== undefined, 'not a valid name!');
	const priceElement = new ItemElement('price', () => true, 'how did we get here?');
	const orderTypeElement = new ItemElement('order-type', () => orderType === 0 || orderType === 1, 'illegal input!');
	const removeAfterElement = new ItemElement('remove-after', () => typeof removeAfter === 'boolean', 'illegal input!');

	onMount(() => {
		for (const element of ItemElement.elements) {
			element.element = document.getElementById(element.id) as HTMLInputElement;
			element.element.addEventListener('invalid', (e) => {
				e.preventDefault();
				const target = e.currentTarget as HTMLInputElement;
				target.classList.add('invalid');
				const id = utility.randomNumber(0, 1000000000);
				const message = target.validationMessage;
				toastsState.push({ id, message, type: 'ERROR' });
			});

			element.element.addEventListener('input', (e) => {
				const target = e.currentTarget as HTMLInputElement;
				target.setCustomValidity('');
				target.classList.remove('invalid');
			});
		}
	});

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();
		let isValid = true;

		for (const element of ItemElement.elements) {
			if (!element.Validate()) {
				isValid = false;
			}
		}

		if (!isValid) {
			return;
		}

		// submission
		const newItem: BazaarItem = {
			ID: id!,
			Name: name,
			OrderType: orderType,
			Price: price,
			RemovedAfter: removeAfter,
			UserId: 0n
		};

		const response = await fetch(action['route'], {
			body: JSON.stringify(newItem),
			method: 'POST'
		});

		console.log(response);

		return e;
	};

	$inspect(id);
	$inspect(name);
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
				<div class="">hello</div>
			</div>
			<div>
				<label for="order-type" class="text-[10px]">Order Type</label>
				<select id="order-type" name="order-type" class="input" bind:value={orderType} required>
					<option value={0}>INSTA</option>
					<option value={1}>ORDER</option>
				</select>
			</div>
			<div>
				<label for="remove-after" class="text-[10px]">Remove After?</label>
				<select id="remove-after" name="remove-after" class="input" bind:value={removeAfter} required>
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

	:global(.invalid) {
		border-color: red;
		animation-iteration-count: 1;
		animation-name: shake;
		animation-duration: 0.25s;
	}
</style>
