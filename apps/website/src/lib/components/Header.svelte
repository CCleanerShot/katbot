<script lang="ts">
	import Link from './Link.svelte';
	import { page } from '$app/state';
	import RoutePage from './RoutePage.svelte';
	import katbotImage from '$lib/images/katbot.png';
	import discordImage from '$lib/images/discord.png';
	import grassBlockImage from '$lib/images/grass_block.png';

	let hover = $state(false);
</script>

<div id="container" class="border-b-2 border-black p-1">
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
	<div class="flex gap-2">
		<RoutePage className="w-32" route="/discord" title="Discord" imageUrl={discordImage} />
		<RoutePage className="w-32" route="/skyblock" title="Skyblock" imageUrl={grassBlockImage} />
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
			width: 3.5rem;
		}
		100% {
			rotate: 360deg;
		}
	}

	img {
		width: 3rem;
		height: 3rem;
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
	}

	h1 {
		font-weight: bold;
		text-align: center;
	}
</style>
