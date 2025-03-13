import type { Routes } from '$lib/other/routes';
import type { BazaarItem } from '$lib/mongodb/BazaarItem';
import type { BazaarItemsAll } from '$lib/mongodb/collections/BazaarItemsAll';

type ApiContract = {
	method: 'GET' | 'POST' | 'DELETE' | 'PATCH';
	response: any;
	route: Routes['API'][number];
	statusCode: number;
	params?: any;
};

// NOTE: I've decided to extract the values outside first so that type-information is more easily readable
const GET_API_AUCTIONS = {
	method: 'GET',
	response: '' as string,
	route: '/api/auctions',
	statusCode: 200,
	params: { userId: 0n as bigint } as const
} as const satisfies ApiContract;
const GET_API_BAZAAR = {
	method: 'GET',
	response: { data: [] as BazaarItemsAll[] } as const,
	route: '/api/bazaar',
	statusCode: 200,
	params: {}
} as const satisfies ApiContract;
const GET_API_BAZAAR_BUY = {
	method: 'GET',
	response: { data: [] as BazaarItem[] } as const,
	route: '/api/bazaar/buy',
	statusCode: 200,
	params: { userId: 0n as bigint } as const
} as const satisfies ApiContract;
const GET_API_BAZAAR_SELL = {
	method: 'GET',
	response: { data: [] as BazaarItem[] } as const,
	route: '/api/bazaar/sell',
	statusCode: 200,
	params: { userId: 0n as bigint } as const
} as const satisfies ApiContract;
const DELETE_API_BAZAAR_BUY = {
	method: 'DELETE',
	response: '' as string,
	route: '/api/bazaar/buy',
	statusCode: 200,
	params: { Name: '' as string } as const
} as const satisfies ApiContract;
const DELETE_API_BAZAAR_SELL = {
	method: 'DELETE',
	response: '' as string,
	route: '/api/bazaar/sell',
	statusCode: 200,
	params: { Name: '' as string } as const
} as const satisfies ApiContract;
const POST_API_BAZAAR_BUY = {
	method: 'POST',
	response: '' as string,
	route: '/?/create-buy-item',
	statusCode: 200,
	params: { item: {} as BazaarItem } as const
} as const satisfies ApiContract;
const POST_API_BAZAAR_SELL = {
	method: 'POST',
	response: '' as string,
	route: '/?/create-sell-item',
	statusCode: 200,
	params: { item: {} as BazaarItem } as const
} as const satisfies ApiContract;

/** Record of API Contracts that map to their cooresponding routes. For now, all routes use the body as params. The status codes represent the normal response. */
export const API_CONTRACTS = {
	'GET=>/api/auctions': GET_API_AUCTIONS,
	'GET=>/api/bazaar': GET_API_BAZAAR,
	'GET=>/api/bazaar/buy': GET_API_BAZAAR_BUY,
	'GET=>/api/bazaar/sell': GET_API_BAZAAR_SELL,
	'DELETE=>/api/bazaar/buy': DELETE_API_BAZAAR_BUY,
	'DELETE=>/api/bazaar/sell': DELETE_API_BAZAAR_SELL,
	'FORM=>/?create/buy': POST_API_BAZAAR_BUY,
	'FORM=>/?create/sell': POST_API_BAZAAR_SELL
} as const satisfies Record<string, ApiContract>;
