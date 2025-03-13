export const ROUTES = {
	API: [
		'/?/create-buy-item',
		'/?/create-sell-item',
		'/api/auctions',
		'/api/auctions/items',
		'/api/bazaar',
		'/api/bazaar/buy',
		'/api/bazaar/sell'
	] as const,
	PAGES: ['/discord', '/skyblock', '/skyblock/auctions', '/skyblock/bazaar'] as const
} as const;

export type Routes = typeof ROUTES;
