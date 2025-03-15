export const ROUTES = {
	API: [
		'/skyblock/?/create-buy-item',
		'/skyblock/?/create-sell-item',
		'/api/auctions',
		'/api/auctions/items',
		'/api/bazaar',
		'/api/bazaar/buy',
		'/api/bazaar/sell',
		'/api/login'
	] as const,
	PAGES: ['/discord', '/skyblock', '/skyblock/auctions', '/skyblock/bazaar'] as const
} as const;

export type Routes = typeof ROUTES;
