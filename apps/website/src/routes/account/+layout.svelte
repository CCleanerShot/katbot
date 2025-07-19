<script lang="ts">
	import RoutePage from '$lib/components/RoutePage.svelte';

	let { children } = $props();
</script>

<div class="flex justify-center gap-2 mt-2">
    <RoutePage route="/account/login" title="Login" classNameText="font-small" thin={true}/>
    <RoutePage route="/account/signup" title="Sign Up" classNameText="font-small" thin={true}/>
</div>

{@render children()}
