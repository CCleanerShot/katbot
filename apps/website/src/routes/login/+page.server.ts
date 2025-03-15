import type { PageServerLoad } from './$types';

export const load: PageServerLoad = (event) => {
	return {
		title: 'Login',
		description: `If you don't know your details, you don't belong here.`
	};
};
