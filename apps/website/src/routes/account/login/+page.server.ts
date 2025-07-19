import type { PageServerLoad } from './$types';

export const load: PageServerLoad = (event) => {
	return {
		title: 'Login',
		description: `Login to use KatBot's site features like stock market tracking for Hypixel items.`
	};
};
