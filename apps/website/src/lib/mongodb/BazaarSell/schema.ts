import { Schema } from 'mongoose';
import { OrderType } from '$lib/enums';
import { type BazaarSell } from '$lib/mongodb/BazaarSell/interface';
import { SVELTE_MONGODB_COLLECTION_BAZAAR_SELL } from '$env/static/private';

export const bazaarSellSchema = new Schema<BazaarSell>(
	{
		ID: String,
		Name: String,
		OrderType: OrderType,
		Price: BigInt,
		RemovedAfter: Boolean,
		UserId: BigInt
	},
	{
		collection: SVELTE_MONGODB_COLLECTION_BAZAAR_SELL,
		toJSON: {
			transform: (doc, ret) => {
				console.log('json', doc, ret);
			}
		},
	}
);
