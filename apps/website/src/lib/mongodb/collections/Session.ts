export type Session = {
	/** The date at which the session key expires. */
	ExpiresAt: Date;
	/** The primary identifier of the session. */
	ID: string;
	/** The discord id the session is for. */
	UserId: bigint;
	/** The username the session is for. */
	Username: string;
};
