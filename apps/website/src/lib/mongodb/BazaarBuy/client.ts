import { Connection, model } from 'mongoose';
import { bazaarBuySchema } from '$lib/mongodb/BazaarBuy/schema';
import {
    SVELTE_MONGODB_BASE_URI,
    SVELTE_MONGODB_COLLECTION_BAZAAR_BUY,
    SVELTE_MONGODB_OPTIONS
} from '$env/static/private';

const connection = new Connection();
connection.openUri(SVELTE_MONGODB_BASE_URI + SVELTE_MONGODB_COLLECTION_BAZAAR_BUY + SVELTE_MONGODB_OPTIONS);

export const bazaarBuyClient = model('bazaar-buys', bazaarBuySchema, SVELTE_MONGODB_COLLECTION_BAZAAR_BUY, {
    connection: connection
});
