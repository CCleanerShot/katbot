<script lang="ts">
	import { modalState } from '$lib/states/modalState.svelte';
	import type { Snippet } from 'svelte';

	type Props = {
		children: Snippet<[]>;
		modal: keyof typeof modalState;
	};

	const { modal, children }: Props = $props();
	const current = modalState[modal];

	const onclick = () => {
		current.isOpened = false;
	};
</script>

<div
	class={[
		'absolute-center inset-0 z-10 backdrop-blur-sm transition-all',
		current.isOpened ? 'visible h-screen w-screen' : 'invisible h-[70vh] w-[70vw] rotate-3 opacity-0'
	]}
>
	<div
		class="absolute-center min-h-1/2 min-w-1/2 inset-0 z-20 flex flex-col rounded-md border-2 border-black bg-white blur-none"
	>
		<div class="flex border-b-2 border-black p-1 font-bold">
			<h3 class="flex-1">ADD A {current.type} ORDER</h3>
			<button {onclick} class="button-border bg-red-500 px-1.5">X</button>
		</div>
		{@render children()}
	</div>
</div>
