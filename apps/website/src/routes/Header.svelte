<script lang="ts">
	import katbotImage from '$lib/images/katbot.png';

	let hover = $state(false);
</script>

<div class="container">
	<img
		alt="katbot logo"
		src={katbotImage}
		class={[{ hover }]}
		onmouseenter={(e) => (hover = true)}
		onanimationiteration={(e) => (hover = false)}
	/>
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

	.container {
		display: flex;
		justify-content: center;
		padding-top: 0.5rem;
	}
</style>
