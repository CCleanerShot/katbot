<script lang="ts">
	import { toastsState } from '$lib/states/toastsState.svelte';
	import Toast from './Toast.svelte';

	let toasts = $state(toastsState);
</script>

<div class="absolute-x-center absolute top-0 z-20">
	{#each toasts as toast}
		<Toast {...toast} />
	{/each}
</div>
