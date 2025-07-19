<script lang="ts">
	import { goto } from '$app/navigation';
	import { base } from '$app/paths';
	import {
		PUBLIC_DISCORD_OAUTH_CLIENT_ID,
		PUBLIC_DISCORD_OAUTH_REDIRECT_URI,
		PUBLIC_DISCORD_OAUTH_REDIRECT_URI_TEST,
		PUBLIC_ENVIRONMENT
	} from '$env/static/public';
	import discordImage from '$lib/images/discord.png';
	import { clientFetch } from '$lib/other/clientFetch';
	import type { OAuthAuthorizeQuery, OAuthAuthorizeQueryDiscord } from '$lib/types';
	import { utility } from '$lib/utility/utility';
	import { redirect } from '@sveltejs/kit';

	type Props = {
		text: string;
		className?: string;
	};

	let { text, className }: Props = $props();

	const onclick = async () => {
		const query: OAuthAuthorizeQueryDiscord = {
			client_id: PUBLIC_DISCORD_OAUTH_CLIENT_ID,
			redirect_uri: utility.getRedirectUri("discord"),
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
