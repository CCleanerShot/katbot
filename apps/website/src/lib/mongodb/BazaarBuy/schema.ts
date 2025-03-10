import { Schema } from 'mongoose';
import { OrderType } from '$lib/enums';
import { type BazaarBuy } from '$lib/mongodb/BazaarBuy/interface';
import { SVELTE_MONGODB_COLLECTION_BAZAAR_BUY } from '$env/static/private';

export const bazaarBuySchema = new Schema<BazaarBuy>(
	{
		ID: String,
		Name: String,
		OrderType: OrderType,
		Price: BigInt,
		RemovedAfter: Boolean,
		UserId: BigInt
	},
	{
		collection: SVELTE_MONGODB_COLLECTION_BAZAAR_BUY,
		toJSON: {
			transform: (doc, ret) => {
				console.log('json', doc, ret);
			}
		}
	}
);
