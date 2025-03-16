import { sha256 } from '@oslojs/crypto/sha2';
import { mongoBot } from '$lib/server/mongoBot';
import type { RequestEvent } from '@sveltejs/kit';
import { MONGODB_SESSION_DAY_LENGTH } from '$env/static/private';
import { encodeBase32LowerCaseNoPadding, encodeHexLowerCase } from '@oslojs/encoding';
import type { Session } from '$lib/mongodb/collections/Session';

// session token: https://lucia-auth.com/sessions/cookies/sveltekit
export const sessionServer = {
	createSession: async (token: string, username: string) => {
		const sessionId = encodeHexLowerCase(sha256(new TextEncoder().encode(token)));
		const days = Number(MONGODB_SESSION_DAY_LENGTH);
		const session: Session = {
			ExpiresAt: new Date(Date.now() + 1000 * 60 * 60 * 24 * days),
			ID: sessionId,
			Username: username
		};

		await mongoBot.MONGODB_C_SESSIONS.InsertOne(session);
		return session;
	},
	deleteDiscordId: (event: RequestEvent) => {
		event.cookies.set('discordId', '', {
			httpOnly: true,
			sameSite: 'lax',
			maxAge: 0,
			path: '/'
		});
	},
	deleteSessionTokenCookie: (event: RequestEvent) => {
		event.cookies.set('session', '', {
			httpOnly: true,
			sameSite: 'lax',
			maxAge: 0,
			path: '/'
		});
	},
	generateSessionToken: (): string => {
		const bytes = new Uint8Array(20);
		crypto.getRandomValues(bytes); // mutates array https://developer.mozilla.org/en-US/docs/Web/API/Crypto/getRandomValues
		const token = encodeBase32LowerCaseNoPadding(bytes);
		return token;
	},
	setDiscordId: (event: RequestEvent, discordId: bigint, expiresAt: Date) => {
		event.cookies.set('discordId', discordId.toString(), {
			httpOnly: true,
			sameSite: 'lax',
			expires: expiresAt,
			path: '/'
		});
	},
	setSessionTokenCookie: (event: RequestEvent, token: string, expiresAt: Date) => {
		event.cookies.set('session', token, {
			httpOnly: true,
			sameSite: 'lax',
			expires: expiresAt,
			path: '/'
		});
	},
	validateSessionToken: async (token: string): Promise<Session | null> => {
		const sessionId = encodeHexLowerCase(sha256(new TextEncoder().encode(token)));
		const session = await mongoBot.MONGODB_C_SESSIONS.FindOne({ id: sessionId });

		if (session === null) {
			return null;
		}

		if (Date.now() >= session.ExpiresAt.getTime()) {
			await mongoBot.MONGODB_C_SESSIONS.DeleteOne({ id: sessionId });
			return null;
		}

		const days = Number(MONGODB_SESSION_DAY_LENGTH);

		if (Date.now() >= session.ExpiresAt.getTime() - 1000 * 60 * 60 * 24 * (days / 2)) {
			session.ExpiresAt = new Date(Date.now() + 1000 * 60 * 60 * 24 * days);
			await mongoBot.MONGODB_C_SESSIONS.UpdateOne({ id: sessionId }, { $set: { ExpiresAt: session.ExpiresAt } });
		}

		return session;
	}
} as const;
