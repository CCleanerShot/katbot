import { Schema } from 'mongoose';
import { SVELTE_MONGODB_COLLECTION_AUCTION_ITEMS } from '$env/static/private';
import type { BazaarItemsAll } from '$lib/mongodb/BazaarItemsAll/interface';

export const bazaarItemsAllSchema = new Schema<BazaarItemsAll>(
	{
		ID: String,
		Name: String
	},
	{
		collection: SVELTE_MONGODB_COLLECTION_AUCTION_ITEMS,
		toJSON: {
			transform: (doc, ret) => {
				console.log('json', doc, ret);
			}
		}
	}
);
