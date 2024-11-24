class CustomFetcher {
	abortController: AbortController;
	constructor() {
		this.abortController = new AbortController();
		this.abortController.signal.addEventListener("abort", (err) => {
			console.log("unhandled err", err);
		});
	}

	async Fetch(input: string | URL | globalThis.Request, init?: RequestInit) {
		return await fetch(input, { ...init, signal: this.abortController.signal });
	}
}

export const myFetcher = new CustomFetcher();
