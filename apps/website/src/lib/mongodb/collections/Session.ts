export type Session = {
	/** The date at which the session key expires. */
	ExpiresAt: Date;
	/** The primary identifier of the session. */
	ID: string;
	/** The auth user id the session is for. */
	UserId: string;
};
