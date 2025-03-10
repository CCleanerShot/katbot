export const ROUTES = {
	API: ['/api/auctions', '/api/auctions/items', '/api/bazaar/buy', '/api/bazaar','/api/bazaar/sell'] as const,
	PAGES: ['/discord', '/skyblock', '/skyblock/auctions', '/skyblock/bazaar'] as const
} as const;

export type Routes = typeof ROUTES;
