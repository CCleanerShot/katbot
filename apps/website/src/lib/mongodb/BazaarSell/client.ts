import mongoose from 'mongoose';
import { bazaarSellSchema } from '$lib/mongodb/BazaarSell/schema';
import {
	SVELTE_MONGODB_BASE_URI,
	SVELTE_MONGODB_COLLECTION_BAZAAR_SELL,
	SVELTE_MONGODB_OPTIONS
} from '$env/static/private';

const connection = new mongoose.Connection();
const uri = SVELTE_MONGODB_BASE_URI + SVELTE_MONGODB_COLLECTION_BAZAAR_SELL + SVELTE_MONGODB_OPTIONS; 
console.log(uri)
connection.openUri(uri);

export const bazaarSellClient = mongoose.model('bazaar-sells', bazaarSellSchema, SVELTE_MONGODB_COLLECTION_BAZAAR_SELL, {
	connection: connection
});
