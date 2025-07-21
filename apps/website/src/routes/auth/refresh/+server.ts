import { type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ request, url, fetch }) => {
	return new Response('ss');
};
