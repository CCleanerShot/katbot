<script lang="ts">
	import Toast from './Toast.svelte';
	import { toastsState } from '$lib/states/toastsState.svelte';

	let toasts = $state(toastsState);
</script>

<div class="absolute-x-center absolute top-0 z-20 mt-0.5 flex flex-col items-center gap-1">
	{#each toasts as toast (toast.id)}
		<Toast {...toast} />
	{/each}
</div>
