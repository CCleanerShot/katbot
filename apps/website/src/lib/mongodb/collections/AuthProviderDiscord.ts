import type { ObjectId } from 'mongodb';

export type AuthProviderDiscord = {
	/** ID of the auth user */
	_id_AuthUser: ObjectId;
	/** Avatar of the discord user */
	Avatar: string;
	/** ID of the discord user*/
	UserId: bigint;
	/** Username of the discord user */
	Username: string;
};
