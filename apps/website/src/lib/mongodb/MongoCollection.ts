import { utility } from '$lib/common/utility';
import { MONGODB_C_AUCTION_BUY } from '$env/static/private';
import { PUBLIC_DOMAIN, PUBLIC_PREFIX_WEBSOCKET } from '$env/static/public';
import {
	Collection,
	ObjectId,
	type Abortable,
	type AnyBulkWriteOperation,
	type BulkWriteOptions,
	type DeleteOptions,
	type Filter,
	type FindOneAndUpdateOptions,
	type FindOptions,
	type InsertOneOptions,
	type OptionalUnlessRequiredId,
	type UpdateFilter,
	type UpdateOptions
} from 'mongodb';

export class MongoCollection<T extends object = object> {
	Collection: Collection<T>;

	constructor(_Collection: Collection<T>) {
		this.Collection = _Collection;

		// "middleware" for the database requests
		if (this.Collection.collectionName === MONGODB_C_AUCTION_BUY) {
			const oldInsertOne = this.InsertOne;
			const oldInsertMany = this.InsertMany;
			const oldUpdateOne = this.UpdateOne;
			const oldUpdateMany = this.UpdateMany;
			const oldUpsertOne = this.UpsertOne;
			this.InsertOne = async (docs, options) => await MongoCollection.Override(this, oldInsertOne, [docs, options]);
			this.InsertMany = async (docs, options) => await MongoCollection.Override(this, oldInsertMany, [docs, options]);
			this.UpdateOne = async (docs, filter, opts) => await MongoCollection.Override(this, oldUpdateOne, [docs, filter, opts]);
			this.UpdateMany = async (docs, filter, opts) => await MongoCollection.Override(this, oldUpdateMany, [docs, filter, opts]);
			this.UpsertOne = async (filter, update, options) => await MongoCollection.Override(this, oldUpsertOne, [filter, update, options]);
		}
	}

	BulkWrite = async (writes: AnyBulkWriteOperation<T>[], options?: BulkWriteOptions) => this.Collection.bulkWrite(writes, options);
	DeleteOne = async (filter?: Filter<T>, options?: DeleteOptions) => this.Collection.deleteOne(filter, options);
	DeleteMany = async (filter?: Filter<T>, options?: DeleteOptions) => this.Collection.deleteMany(filter, options);
	InsertOne = async (doc: OptionalUnlessRequiredId<T>, options?: InsertOneOptions) => this.Collection.insertOne(doc, options);
	InsertMany = async (docs: OptionalUnlessRequiredId<T>[], options?: BulkWriteOptions) => this.Collection.insertMany(docs, options);
	UpdateOne = async (docs: Filter<T>, filter: UpdateFilter<T>, opts?: UpdateOptions) => this.Collection.updateOne(docs, filter, opts);
	UpdateMany = async (docs: Filter<T>, filter: UpdateFilter<T>, opts?: UpdateOptions) => this.Collection.updateMany(docs, filter, opts);

	static async Override<T extends (...args: any) => Promise<any>, K extends Parameters<T>>(
		bound: unknown,
		func: T,
		args: K
	): Promise<ReturnType<T>> {
		const response = await func.call(bound, ...args);

		try {
			if (response?.acknowledged || response === null) {
				utility.reverseString;
				await fetch(`${utility.getProtocol('http')}://${PUBLIC_PREFIX_WEBSOCKET}.${PUBLIC_DOMAIN}`);
			}
		} catch (err) {
			// doesn't really matter if the other server is down
		}

		return response;
	}

	async Find(filter?: Filter<T>, options?: FindOptions & Abortable): Promise<T[]> {
		const response = filter ? this.Collection.find(filter, options) : this.Collection.find();
		const array = await response.toArray();

		for (const item of array) {
			delete (item as any)._id;
		}

		return array as T[];
	}

	async FindOne(filter: Filter<T>, options?: FindOptions & Abortable): Promise<(T & { _id: ObjectId }) | null> {
		const response = await this.Collection.findOne(filter, options);
		return response as any as Promise<(T & { _id: ObjectId }) | null>;
	}

	async UpsertOne(filter: Filter<T>, update: UpdateFilter<T> | Document[], options: FindOneAndUpdateOptions): Promise<T | null> {
		const response = await this.Collection.findOneAndUpdate(filter, update, options);

		if (response !== null) {
			delete (response as any)._id;
		}

		return response as T | null;
	}
}
