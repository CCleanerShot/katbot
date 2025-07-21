export const ROUTES = {
	API: ['/api/auctions', '/api/auctions/buy', '/api/bazaar', '/api/bazaar/buy', '/api/bazaar/sell', '/api/login'] as const,
	PAGES: ['/discord', '/skyblock', '/skyblock/auctions', '/skyblock/bazaar', '/account/login', '/account/signup'] as const
} as const;

export type Routes = typeof ROUTES;
