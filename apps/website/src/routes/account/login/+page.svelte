<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import { goto } from '$app/navigation';
	import { Form } from '$lib/classes/Form.svelte';
	import { FormElement } from '$lib/classes/FormElement.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';

	let username = $state('');
	let password = $state('');

	const eUsername = new FormElement('username', () => ({ success: true }));
	const ePassword = new FormElement('password', () => ({ success: true }));
	const form = new Form([eUsername, ePassword]);

	onMount(() => {
		form.Mount();

		return () => {
			form.Clean();
		};
	});

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();

		if (!form.IsValid()) {
			return;
		}

		const response = await utilityClient.fetch(
			'POST=>/api/login',
			{ user: { Username: username, Password: password, Provider: 'email' } },
			true
		);

		if (response.ok) {
			const redirect = page.url.searchParams.get('redirect');
			await goto(window.location.origin + (redirect ?? '/'));
		}
	};
</script>

<div class="flex flex-col items-center gap-2">
	<h2 class="mt-2">Login via email or <a class="font-bold underline" href="https://discord.com/">Discord</a></h2>
	<form method="POST" class="flex flex-col items-center gap-1 border-4 border-black bg-white p-1" {onsubmit}>
		<div>
			<label for="username">USERNAME</label>
			<input bind:value={username} class="input" id="username" name="username" type="text" />
		</div>
		<div>
			<label for="password">PASSWORD</label>
			<input bind:value={password} class="input" id="password" name="password" type="password" />
		</div>
		<button type="submit" class="button mt-1">SUBMIT</button>
	</form>
</div>

<style>
	* {
		font-size: 0.5rem;
	}

	label:hover {
		text-decoration: underline;
		cursor: pointer;
	}
</style>
