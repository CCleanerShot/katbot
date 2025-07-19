import { toastActions } from '$lib/states/toastsState.svelte';
import type { FormElement } from '$lib/classes/FormElement.svelte';

export class Form {
	elements: FormElement[];

	constructor(elements: FormElement[]) {
		this.elements = elements;

		for (const element of this.elements) {
			element.form = this;
		}
	}

	public Clean() {
		for (const element of this.elements) {
			element.element.removeEventListener('invalid', this._invalid);
			element.element.removeEventListener('input', this._invalid);
		}
	}

	public Mount() {
		for (const element of this.elements) {
			element.element = document.getElementById(element.id) as HTMLInputElement;
			element.element.addEventListener('invalid', this._invalid);
			element.element.addEventListener('input', this._input);
		}
	}

	public IsValid(): boolean {
		let isValid = true;

		for (const element of this.elements) {
			if (!element.Validate()) {
				isValid = false;
			}
		}

		return isValid;
	}

	private _invalid(e: Event) {
		e.preventDefault();
		const target = e.currentTarget as HTMLInputElement;
		target.classList.add('invalid');
		toastActions.addToast({ message: `(${target.id}) ${target.validationMessage}`, type: 'ERROR' });
	}

	private _input(e: Event) {
		const target = e.currentTarget as HTMLInputElement;
		target.setCustomValidity('');
		target.classList.remove('invalid');
	}
}
