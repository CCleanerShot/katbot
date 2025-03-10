<script lang="ts">
	import katbotImage from '$lib/images/katbot.png';
	import { page } from '$app/state';
	import RoutePage from './RoutePage.svelte';
	let hover = $state(false);
	import discordImage from '$lib/images/discord.png';
	import grassBlockImage from '$lib/images/grass_block.png';
	import Link from './Link.svelte';
</script>

<div id="container" class="border-b-2 border-black p-2">
	<Link href="/">
		<img
			alt="katbot logo"
			src={katbotImage}
			class={[{ hover }]}
			onmouseenter={(e) => (hover = true)}
			onanimationiteration={(e) => (hover = false)}
		/>
	</Link>
	<h1>KatBot</h1>
	{#if page.route.id === '/'}
		<div>here lies: useless crap</div>
	{/if}
	<div class="flex gap-2 *:w-28">
		<RoutePage route="/discord" title="Discord" imageUrl={discordImage} />
		<RoutePage route="/skyblock" title="Skyblock" imageUrl={grassBlockImage} />
	</div>
</div>

<style>
	@keyframes spin-logo {
		0% {
			rotate: 0deg;
		}
		50% {
			rotate: 365deg;
			scale: 110%;
			width: 4rem;
		}
		100% {
			rotate: 360deg;
		}
	}

	img {
		width: 4rem;
		height: 4rem;
	}

	img.hover {
		animation-name: spin-logo;
		animation-duration: 1s;
		animation-iteration-count: infinite;
	}

	#container {
		display: flex;
		flex-direction: column;
		align-items: center;
		padding-top: 0.5rem;
	}

	h1 {
		font-weight: bold;
		text-align: center;
	}
</style>
