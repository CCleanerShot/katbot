<script lang="ts">
	import { PUBLIC_DISCORD_OAUTH_CLIENT_ID } from '$env/static/public';
	import discordImage from '$lib/images/discord.png';
	import type { OAuthAuthorizeQuery } from '$lib/types';
	import { utility } from '$lib/common/utility'

	type Props = {
		text: string;
		className?: string;
	};

	let { text, className }: Props = $props();

	const onclick = async () => {
		// list of valid scopes https://discord.com/developers/docs/topics/oauth2
		const query: OAuthAuthorizeQuery & { scope: 'identify' } = {
			client_id: PUBLIC_DISCORD_OAUTH_CLIENT_ID,
			redirect_uri: utility.getRedirectUri('discord'),
			response_type: 'code',
			scope: 'identify'
		};

		const baseUrl = new URL('https://discord.com/oauth2/authorize');

		for (const [key, value] of utility.betterEntries(query)) {
			baseUrl.searchParams.set(key, value);
		}

		window.location.href = baseUrl.toString();
	};
</script>

<button class={['button button-thin flex items-center gap-2', className]} {onclick}>
	<span class="font-xx-small">{text}</span>
	<img src={discordImage} alt="Icon for Discord" width={18} />
</button>

<style>
	.button:hover {
		background-color: var(--color-discord-foreground);
	}

	.button:hover img {
		filter: grayscale(100%);
	}
</style>
