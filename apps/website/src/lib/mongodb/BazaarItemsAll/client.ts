import { Connection, model } from 'mongoose';
import { bazaarItemsAllSchema } from '$lib/mongodb/BazaarItemsAll/schema';
import {
    SVELTE_MONGODB_BASE_URI,
    SVELTE_MONGODB_COLLECTION_BAZAAR_ITEMS,
    SVELTE_MONGODB_OPTIONS
} from '$env/static/private';

const connection = new Connection();
connection.openUri(SVELTE_MONGODB_BASE_URI + SVELTE_MONGODB_COLLECTION_BAZAAR_ITEMS + SVELTE_MONGODB_OPTIONS);

export const bazaarItemsAllClient = model('bazaar-items', bazaarItemsAllSchema, SVELTE_MONGODB_COLLECTION_BAZAAR_ITEMS, {
    connection: connection
});
