import { config } from "dotenv";
config();

/**
 * Holds our environment variables
 */
export class BotEnvironment {
	static DISCORD_APPLICATION_ID: string = process.env.DISCORD_APPLICATION_ID!;
	static DISCORD_MAIN_CHANNEL_ID: string = process.env.DISCORD_MAIN_CHANNEL_ID!;
	static DISCORD_PUBLIC_KEY: string = process.env.DISCORD_PUBLIC_KEY!;
	static DISCORD_TOKEN: string = process.env.DISCORD_TOKEN!;
	static HYPIXEL_BOT_KEY: string = process.env.HYPIXEL_BOT_KEY!;
	static SUPABASE_DATABASE_PASSWORD: string = process.env.SUPABASE_DATABASE_PASSWORD!;
	static SUPABASE_PUBLIC_KEY: string = process.env.SUPABASE_PUBLIC_KEY!;
	static SUPABASE_SERVICE_ROLE: string = process.env.SUPABASE_SERVICE_ROLE!;
	static SUPABASE_URL: string = process.env.SUPABASE_URL!;
}
