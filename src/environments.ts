import { config } from "dotenv";
config();

/**
 * Holds our environment variables
 */
export class Environment {
	static DISCORD_APPLICATION_ID = process.env.DISCORD_APPLICATION_ID!;
	static DISCORD_PUBLIC_KEY = process.env.DISCORD_PUBLIC_KEY!;
	static DISCORD_TOKEN = process.env.DISCORD_TOKEN!;
	static HYPIXEL_BOT_KEY = process.env.HYPIXEL_BOT_KEY!;
}
