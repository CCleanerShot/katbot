import type { Form } from './Form.svelte';

export class FormElement {
	public element: HTMLInputElement;
	public id: string;
	public form?: Form;
	private validation: (form: Form) => { success: true } | { success: false; msg: string };

	public constructor(id: string, validation: (form: Form) => { success: true } | { success: false; msg: string }) {
		this.id = id;
		this.element = undefined as any as HTMLInputElement; // gets defined during onMount;
		this.validation = validation;
	}

	/** Contains a side-effect which sets the input's custom validity message and reports it, if false. */
	public Validate(): boolean {
		if (!this.form) {
			return false;
		}

		const result = this.validation(this.form);
		if (result.success) {
			return true;
		} else {
			this.element.setCustomValidity(result.msg);
			this.element.reportValidity();
			return false;
		}
	}
}
