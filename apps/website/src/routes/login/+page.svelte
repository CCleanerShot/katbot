<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import { goto } from '$app/navigation';
	import { Form } from '$lib/classes/Form.svelte';
	import { clientFetch } from '$lib/other/clientFetch';
	import { FormElement } from '$lib/classes/FormElement.svelte';
	import type { MongoUser } from '$lib/mongodb/collections/MongoUser';
	import { fly } from 'svelte/transition';

	let username: MongoUser['Username'] = $state('');
	let password: MongoUser['Password'] = $state('');

	const form = new Form([
		new FormElement('username', () => true, 'how did we get here?'),
		new FormElement('password', () => true, 'how did we get here?')
	]);

	let test = $state(true)
	
	onMount(() => {
		form.Mount();

		return () => {
			form.Clean();
		};
	});

	$effect(() => {
		setTimeout(() => {
			test = !test;
			console.log(test)
		}, 1000)
	})

	const onsubmit = async (e: SubmitEvent) => {
		e.preventDefault();

		if (!form.IsValid()) {
			return;
		}

		const response = await clientFetch('POST=>/api/login', { user: { Username: username, Password: password } }, true);

		if (response.ok) {
			const redirect = page.url.searchParams.get('redirect');
			await goto(window.location.origin + (redirect ?? '/'));
		}
	};
</script>

<div class="flex flex-col items-center gap-2">
	<h2 class="mt-1">If you don't know your details, you don't belong here.</h2>
	<form method="POST" class="flex flex-col items-center gap-1 border-4 border-black bg-white p-1" {onsubmit}>
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
{#if test}
	<div transition:fly>test</div>
{/if}

<style>
	* {
		font-size: 0.5rem;
	}

	label:hover {
		text-decoration: underline;
		cursor: pointer;
	}
</style>
