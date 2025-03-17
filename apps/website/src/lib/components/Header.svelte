<script lang="ts">
	import Link from './Link.svelte';
	import { page } from '$app/state';
	import RoutePage from './RoutePage.svelte';
	import katbotImage from '$lib/images/katbot.png';
	import discordImage from '$lib/images/discord.png';
	import grassBlockImage from '$lib/images/grass_block.png';

	let hover = $state(false);

	let indexPage = $derived(page.route.id === '/');
</script>

<div id="container" class={['border-b-2 border-black p-2', indexPage ? '' : 'small']}>
	<div class="flex gap-4 pb-2">
		<Link href="/">
			<img
				alt="katbot logo"
				src={katbotImage}
				class={[{ hover }]}
				onmouseenter={(e) => (hover = true)}
				onanimationiteration={(e) => (hover = false)}
			/>
		</Link>
		<h1 class="m-auto">KatBot</h1>
	</div>
	{#if indexPage}
		<div>here lies: useless crap</div>
	{/if}
	<div class="flex gap-2">
		<RoutePage className="w-48" route="/discord" title="Discord" imageUrl={discordImage} />
		<RoutePage className="w-48" route="/skyblock" title="Skyblock" imageUrl={grassBlockImage} />
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
			width: 5rem;
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
	}

	h1 {
		font-weight: bold;
		text-align: center;
	}
</style>
