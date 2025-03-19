import {
	Collection,
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
	}

	BulkWrite = async (writes: AnyBulkWriteOperation<T>[], options?: BulkWriteOptions) => this.Collection.bulkWrite(writes, options);
	DeleteOne = async (filter?: Filter<T>, options?: DeleteOptions) => this.Collection.deleteOne(filter, options);
	DeleteMany = async (filter?: Filter<T>, options?: DeleteOptions) => this.Collection.deleteMany(filter, options);
	InsertOne = async (doc: OptionalUnlessRequiredId<T>, options?: InsertOneOptions) => this.Collection.insertOne(doc, options);
	InsertMany = async (docs: OptionalUnlessRequiredId<T>[], options?: BulkWriteOptions) => this.Collection.insertMany(docs, options);
	UpdateOne = async (docs: Filter<T>, filter: UpdateFilter<T>, opts?: UpdateOptions) => this.Collection.updateOne(docs, filter, opts);
	UpdateMany = async (docs: Filter<T>, filter: UpdateFilter<T>, opts?: UpdateOptions) => this.Collection.updateMany(docs, filter, opts);

	async Find(filter?: Filter<T>, options?: FindOptions & Abortable): Promise<T[]> {
		const response = filter ? this.Collection.find(filter, options) : this.Collection.find();
		const array = await response.toArray();

		for (const item of array) {
			delete (item as any)._id;
		}

		return array as T[];
	}

	async FindOne(filter: Filter<T>, options?: FindOptions & Abortable): Promise<T | null> {
		const response = await this.Collection.findOne(filter, options);

		// TODO: log the condition that the response is null, or otherwise
		if (response == null) {
			return null;
		} else {
			return response;
		}
	}

	async UpsertOne(filter: Filter<T>, update: UpdateFilter<T> | Document[], options: FindOneAndUpdateOptions): Promise<T | null> {
		const response = await this.Collection.findOneAndUpdate(filter, update, options);

		// TODO: log the condition that the response is null, or otherwise
		if (response == null) {
			return null;
		} else {
			delete (response as any)._id;
			return response as T;
		}
	}
}
