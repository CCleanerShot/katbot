import type { ObjectId } from 'mongodb';

export type MongoUser = {
	/** ID of the auth provider */
	AuthorizationId: ObjectId;
	/** Source of the auth provider. Used for simplicity, rather than searching all providers for an associated ID. */
	AuthorizationSource: string;
};
