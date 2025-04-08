<script lang="ts">
	import { onMount, type Snippet } from 'svelte';

	type Props = {
		children: Snippet<[]>;
        isOpened: boolean;
		className?: string;
	};

	let element: HTMLDivElement;
	let { children, isOpened, className }: Props = $props();

	onMount(() => {
		const moveSelf = (ev: MouseEvent) => {
			const rect = element.getBoundingClientRect();
			element.style.top = `${Math.min(ev.pageY, document.body.clientHeight - rect.height)}px`;
			element.style.left = `${Math.min(ev.pageX, document.body.clientWidth - rect.width)}px`;
		};

		window.addEventListener('mousemove', moveSelf);

		return () => {
			window.removeEventListener('mousemove', moveSelf);
		};
	});
</script>

<div bind:this={element} class={[!isOpened ? "hidden" : "",'font-small absolute flex flex-col rounded-sm border-2 bg-white p-0.5', className]}>
	<span class="italic font-x-small-recursive border-b text-right"><strong>ESC</strong> => CLOSE</span>
	{@render children()}
</div>
