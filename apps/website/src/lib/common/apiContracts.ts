import type { Routes } from '$lib/common/routes';
import type { BazaarItem } from '$lib/mongodb/BazaarItem';
import type { BazaarBuy } from '$lib/mongodb/collections/BazaarBuy';
import type { AuthUser } from '$lib/mongodb/collections/AuthUser';
import type { BazaarSell } from '$lib/mongodb/collections/BazaarSell';
import type { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
import type { AuctionTags } from '$lib/mongodb/collections/AuctionTags';
import type { BazaarItemsAll } from '$lib/mongodb/collections/BazaarItemsAll';
import type { AuctionItemsAll } from '$lib/mongodb/collections/AuctionItemsAll';
import type { Provider } from '$lib/types';
import type { AuthProviderDiscord } from '$lib/mongodb/collections/AuthProviderDiscord';

type ApiContract = {
	method: 'GET' | 'POST' | 'DELETE' | 'PUT' | 'PUT';
	response: any;
	route: Routes['API'][number];
	params?: any;
};

// NOTE: I've decided to extract the values outside first so that type-information is more easily readable
const GET_API_AUCTIONS = {
	method: 'GET',
	response: { data: { items: [] as AuctionItemsAll[], tags: [] as AuctionTags[] } } as const,
	route: '/api/auctions',
	params: {} as const
} as const satisfies ApiContract;
const GET_API_AUCTIONS_BUY = {
	method: 'GET',
	response: { data: [] as AuctionBuy[] } as const,
	route: '/api/auctions/buy',
	params: {} as const
} as const satisfies ApiContract;
const GET_API_BAZAAR = {
	method: 'GET',
	response: { data: [] as BazaarItemsAll[] } as const,
	route: '/api/bazaar',
	params: {}
} as const satisfies ApiContract;
const GET_API_BAZAAR_BUY = {
	method: 'GET',
	response: { data: [] as BazaarBuy[] } as const,
	route: '/api/bazaar/buy',
	params: {}
} as const satisfies ApiContract;
const GET_API_BAZAAR_SELL = {
	method: 'GET',
	response: { data: [] as BazaarItem[] } as const,
	route: '/api/bazaar/sell',
	params: {} as const
} as const satisfies ApiContract;
const DELETE_API_AUCTIONS_BUY = {
	method: 'DELETE',
	response: '' as string,
	route: '/api/auctions/buy',
	params: { ID: '' as string } as const
} as const satisfies ApiContract;
const DELETE_API_BAZAAR_BUY = {
	method: 'DELETE',
	response: '' as string,
	route: '/api/bazaar/buy',
	params: { ID: '' as string } as const
} as const satisfies ApiContract;
const DELETE_API_BAZAAR_SELL = {
	method: 'DELETE',
	response: '' as string,
	route: '/api/bazaar/sell',
	params: { ID: '' as string } as const
} as const satisfies ApiContract;
const PUT_API_AUCTIONS_BUY = {
	method: 'PUT',
	response: '' as string,
	route: '/api/auctions/buy',
	params: { items: [] as Partial<Omit<AuctionBuy, 'UserId'>>[] } as const
} as const satisfies ApiContract;
const PUT_API_BAZAAR_BUY = {
	method: 'PUT',
	response: '' as string,
	route: '/api/bazaar/buy',
	params: { items: [] as Partial<Omit<BazaarBuy, 'UserId'>>[] } as const
} as const satisfies ApiContract;
const PUT_API_BAZAAR_SELL = {
	method: 'PUT',
	response: '' as string,
	route: '/api/bazaar/sell',
	params: { items: [] as Partial<Omit<BazaarSell, 'UserId'>>[] } as const
} as const satisfies ApiContract;
const POST_API_AUCTIONS_BUY = {
	method: 'POST',
	response: '' as string,
	route: '/api/auctions/buy',
	params: { item: {} as Omit<AuctionBuy, 'UserId'> } as const
} as const satisfies ApiContract;
const POST_API_BAZAAR_BUY = {
	method: 'POST',
	response: '' as string,
	route: '/api/bazaar/buy',
	params: { item: {} as Omit<BazaarBuy, 'UserId'> } as const
} as const satisfies ApiContract;
const POST_API_BAZAAR_SELL = {
	method: 'POST',
	response: '' as string,
	route: '/api/bazaar/sell',
	params: { item: {} as Omit<BazaarSell, 'UserId'> } as const
} as const satisfies ApiContract;
const POST_API_LOGIN = {
	method: 'POST',
	response: '' as string,
	route: '/api/login',
	params: { user: {} as Pick<AuthUser, 'Password'> & { Provider: Provider; Username: string } } as const
} as const satisfies ApiContract;

/** Record of API Contracts that map to their cooresponding routes. For now, all routes use the body as params. The status codes represent the normal response. */
export const API_CONTRACTS = {
	'GET=>/api/auctions': GET_API_AUCTIONS,
	'GET=>/api/auctions/buy': GET_API_AUCTIONS_BUY,
	'GET=>/api/bazaar': GET_API_BAZAAR,
	'GET=>/api/bazaar/buy': GET_API_BAZAAR_BUY,
	'GET=>/api/bazaar/sell': GET_API_BAZAAR_SELL,
	'DELETE=>/api/auctions/buy': DELETE_API_AUCTIONS_BUY,
	'DELETE=>/api/bazaar/buy': DELETE_API_BAZAAR_BUY,
	'DELETE=>/api/bazaar/sell': DELETE_API_BAZAAR_SELL,
	'PUT=>/api/auctions/buy': PUT_API_AUCTIONS_BUY,
	'PUT=>/api/bazaar/buy': PUT_API_BAZAAR_BUY,
	'PUT=>/api/bazaar/sell': PUT_API_BAZAAR_SELL,
	'POST=>/api/auctions/buy': POST_API_AUCTIONS_BUY,
	'POST=>/api/bazaar/buy': POST_API_BAZAAR_BUY,
	'POST=>/api/bazaar/sell': POST_API_BAZAAR_SELL,
	'POST=>/api/login': POST_API_LOGIN
} as const satisfies Record<string, ApiContract>;
