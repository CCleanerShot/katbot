<script lang="ts">
	import '../app.css';
	import Header from './Header.svelte';

	let { children } = $props();
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Katbot's site" />
</svelte:head>

<main>
	<Header />
	{@render children()}
</main>
