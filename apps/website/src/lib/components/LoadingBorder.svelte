<script lang="ts">
	import { fetchState } from '$lib/states/fetchState.svelte';

	let status = $derived(fetchState.status);
</script>

<div class={['parent z-50 transition-all', status === 'error' ? 'red' : status === 'loading' ? 'yellow' : 'green']}>
	<div class="top-left"></div>
	<div class="top-right"></div>
	<div class="bottom-left"></div>
	<div class="bottom-right"></div>
</div>

<style>
	:global(.green) {
		--color: green;
	}

	:global(.yellow) {
		--color: yellow;
	}

	:global(.red) {
		--color: red;
	}

	.top-left {
		top: 0px;
		left: 0px;
		border-top-color: var(--color);
		border-left-color: var(--color);
		border-bottom-color: transparent;
		border-right-color: transparent;
	}

	.top-right {
		top: 0px;
		right: 0px;
		border-top-color: var(--color);
		border-left-color: transparent;
		border-bottom-color: transparent;
		border-right-color: var(--color);
	}

	.bottom-left {
		bottom: 0px;
		left: 0px;
		border-top-color: transparent;
		border-left-color: var(--color);
		border-bottom-color: var(--color);
		border-right-color: transparent;
	}

	.bottom-right {
		bottom: 0px;
		right: 0px;
		border-top-color: transparent;
		border-left-color: transparent;
		border-bottom-color: var(--color);
		border-right-color: var(--color);
	}

	.parent div {
		border-width: 10px;
		position: absolute;
		transition-property: border-width;
		transition-duration: 0.2s;
	}

	.parent.yellow div {
		border-width: 20px;
	}
</style>
