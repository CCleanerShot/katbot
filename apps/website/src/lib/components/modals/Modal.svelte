<script lang="ts">
	import { modalState } from '$lib/states/modalState.svelte';
	import type { Snippet } from 'svelte';

	type Props = {
		children: Snippet<[]>;
		modal: keyof typeof modalState;
		title: string;
		id?: string;
	};

	const { children, modal, title, id }: Props = $props();
	const current = modalState[modal];

	const onclick = () => {
		current.isOpened = false;
	};
</script>

<div
	{id}
	class={[
		'absolute-center z-10 backdrop-blur-sm transition-all',
		current.isOpened ? 'visible h-screen w-screen' : 'invisible h-[70vh] w-[70vw] rotate-3 opacity-0'
	]}
>
	<div class="absolute-center z-20 flex min-h-[75vh] min-w-[75vw] flex-col rounded-md border-4 border-black bg-white blur-none">
		<div class="flex border-b-2 border-black p-2 font-bold">
			<h3 class="flex-1">{title}</h3>
			<button {onclick} class="button-border bg-red-500 px-3">X</button>
		</div>
		{@render children()}
	</div>
</div>
