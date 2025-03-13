import type { Actions } from '@sveltejs/kit';

// TODO: move elsewhere this is a very awkward spot
export const actions = {
	'create-buy-item': async (event) => {
		const data = await event.request.formData();
		console.log(data.get('name'));
	},
	'create-sell-item': async (event) => {
		const data = await event.request.formData();
		console.log(data.get('name'));
	}
} satisfies Actions;
