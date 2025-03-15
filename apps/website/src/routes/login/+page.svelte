<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import { goto } from '$app/navigation';
	import type { User } from '$lib/types';
	import { Form } from '$lib/classes/Form.svelte';
	import { clientFetch } from '$lib/other/clientFetch';
	import { FormElement } from '$lib/classes/FormElement.svelte';

	let username: User['username'] = $state('');
	let password: User['password'] = $state('');

	const form = new Form([
		new FormElement('username', () => true, 'how did we get here?'),
		new FormElement('password', () => true, 'how did we get here?')
	]);

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

		const response = await clientFetch('POST=>/api/login', { user: { username, password } }, true);

		if (response.ok) {
			const redirect = page.url.searchParams.get('redirect');
			await goto('http://localhost:5173/skyblock');
		}
	};
</script>

<div class="flex flex-col items-center gap-2">
	<h2 class="mt-1">If you don't know your details, you don't belong here.</h2>
	<form method="POST" class="flex flex-col items-center gap-1 border-2 border-black bg-white p-1" {onsubmit}>
		<div>
			<label for="username">USERNAME</label>
			<input bind:value={username} class="input" id="username" name="username" type="text" />
		</div>
		<div>
			<label for="password">PASSWORD</label>
			<input bind:value={password} class="input" id="password" name="password" type="password" />
		</div>
		<button type="submit" class="button mt-1">LOGIN</button>
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
