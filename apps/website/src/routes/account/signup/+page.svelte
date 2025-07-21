<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/state';
	import { goto } from '$app/navigation';
	import { Form } from '$lib/classes/Form.svelte';
	import { FormElement } from '$lib/classes/FormElement.svelte';
	import { utilityClient } from '$lib/client/utilityClient.svelte';
	import DiscordProvider from '$lib/components/oauth/DiscordProvider.svelte';

	let username = $state('');
	let password = $state('');

	const stringUsername = 'username';
	const stringPassword = 'password';
	const stringPasswordConfirmation = 'password-confirmation';
	const formUsername = new FormElement(stringUsername, () => ({ success: true }));
	const formPassword = new FormElement(stringPassword, () => ({ success: true }));
	const formPasswordConfirm = new FormElement(stringPasswordConfirmation, (form) => {
		const passwordE = form.elements.find((e) => e.id === stringPassword);

		if (!passwordE) {
			return { success: false, msg: 'Unknown error: Attempted to find the password field, but it is missing!' };
		}

		const test = passwordE.element.textContent;
		console.log(test);

		return { success: true };
	});

	const form = new Form([formUsername, formPassword, formPasswordConfirm]);

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

		const response = await utilityClient.fetch('POST=>/api/login', { user: { Password: password, Provider: "email", Username: username } }, true);

		if (response.ok) {
			const redirect = page.url.searchParams.get('redirect');
			await goto(window.location.origin + (redirect ?? '/'));
		}
	};
</script>

<div class="flex flex-col items-center gap-2 p-2">
	<DiscordProvider text="Sign Up w/ Discord"/>
	<form method="POST" class="flex flex-col items-center gap-1 border-4 border-black bg-white p-1" {onsubmit}>
		<div>
			<label for={stringUsername}>USERNAME</label>
			<input bind:value={username} class="input" id={stringUsername} name={stringUsername} type="text" />
		</div>
		<div>
			<label for={stringPassword}>PASSWORD</label>
			<input bind:value={password} class="input" id={stringPassword} name={stringPassword} type="password" />
		</div>
		<div>
			<label for={stringPasswordConfirmation}>CONFIRM PASSWORD</label>
			<input bind:value={password} class=" input" id={stringPasswordConfirmation} name={stringPasswordConfirmation} type="password" />
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

	form > div {
		align-items: center;
		display: flex;
		gap: 4px;
		justify-content: space-between;
		width: 100%;
	}
</style>
