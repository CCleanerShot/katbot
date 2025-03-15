export class FormElement {
	public element: HTMLInputElement;
	public id: string;
	validation: () => boolean;
	invalidMessage: string;

	constructor(id: string, validation: () => boolean, invalidMessage: string) {
		this.id = id;
		this.element = undefined as any as HTMLInputElement; // gets defined during onMount;
		this.validation = validation;
		this.invalidMessage = invalidMessage;
	}

	/** Contains a side-effect which sets the input's custom validity message and reports it, if false. */
	public Validate(): boolean {
		if (this.validation()) {
			return true;
		} else {
			this.element.setCustomValidity(this.invalidMessage);
			this.element.reportValidity();
			return false;
		}
	}
}
