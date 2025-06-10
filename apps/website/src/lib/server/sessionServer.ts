import { sha256 } from '@oslojs/crypto/sha2';
import { mongoBot } from '$lib/server/mongoBot';
import type { RequestEvent } from '@sveltejs/kit';
import type { Session } from '$lib/mongodb/collections/Session';
import { MONGODB_SESSION_DAY_LENGTH } from '$env/static/private';
import { encodeBase32LowerCaseNoPadding, encodeHexLowerCase } from '@oslojs/encoding';
import { PUBLIC_DOMAIN } from '$env/static/public';

// session token: https://lucia-auth.com/sessions/cookies/sveltekit
export const sessionServer = {
	createSession: async (token: string, userId: bigint, username: string) => {
		const sessionId = encodeHexLowerCase(sha256(new TextEncoder().encode(token)));
		const days = Number(MONGODB_SESSION_DAY_LENGTH);
		const session: Session = {
			ExpiresAt: new Date(Date.now() + 1000 * 60 * 60 * 24 * days),
			ID: sessionId,
			UserId: userId,
			Username: username
		};

		await mongoBot.MONGODB_C_SESSIONS.InsertOne(session);
		return session;
	},
	deleteDiscordId: (event: RequestEvent) => {
		event.cookies.set('discordId', '', {
			sameSite: 'lax',
			maxAge: 0,
			path: '/'
		});
	},
	deleteSessionTokenCookie: (event: RequestEvent) => {
		event.cookies.set('session', '', {
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
			sameSite: 'none',
			expires: expiresAt,
			path: '/'
		});
	},
	setDomain: (event: RequestEvent) => {
		event.cookies.set('domain', PUBLIC_DOMAIN, {
			sameSite: 'none',
			expires: new Date(Date.now() + 1000 * 60 * 60 * 24 * 30),
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

		const header = event.request.headers.get('Set-Cookie');
		event.request.headers.set('Set-Cookie', header + ` session=${token};`);
	},

	validateSessionToken: async (token: string): Promise<Session | null> => {
		const sessionId = encodeHexLowerCase(sha256(new TextEncoder().encode(token)));
		const session = await mongoBot.MONGODB_C_SESSIONS.FindOne({ ID: sessionId });

		if (session === null) {
			return null;
		}

		if (Date.now() >= session.ExpiresAt.getTime()) {
			await mongoBot.MONGODB_C_SESSIONS.DeleteOne({ ID: sessionId });
			return null;
		}

		const days = Number(MONGODB_SESSION_DAY_LENGTH);

		if (Date.now() >= session.ExpiresAt.getTime() - 1000 * 60 * 60 * 24 * (days / 2)) {
			session.ExpiresAt = new Date(Date.now() + 1000 * 60 * 60 * 24 * days);
			await mongoBot.MONGODB_C_SESSIONS.UpdateOne({ ID: sessionId }, { $set: { ExpiresAt: session.ExpiresAt } });
		}

		return session;
	}
} as const;
