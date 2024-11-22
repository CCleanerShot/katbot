import { config } from "dotenv";
config();

/**
 * Holds our environment variables
 */
export const BotEnvironment = {
	ADMIN_1: process.env.ADMIN_1!,
	ADMIN_2: process.env.ADMIN_2!,
	DISCORD_APPLICATION_ID: process.env.DISCORD_APPLICATION_ID!,
	DISCORD_MAIN_CHANNEL_ID: process.env.DISCORD_MAIN_CHANNEL_ID!,
	DISCORD_PUBLIC_KEY: process.env.DISCORD_PUBLIC_KEY!,
	DISCORD_SEND_CHANNEL_ID: process.env.DISCORD_SEND_CHANNEL_ID!,
	DISCORD_TOKEN: process.env.DISCORD_TOKEN!,
	HYPIXEL_BOT_KEY: process.env.HYPIXEL_BOT_KEY!,
	SUPABASE_DATABASE_PASSWORD: process.env.SUPABASE_DATABASE_PASSWORD!,
	SUPABASE_PUBLIC_KEY: process.env.SUPABASE_PUBLIC_KEY!,
	SUPABASE_SERVICE_ROLE: process.env.SUPABASE_SERVICE_ROLE!,
	SUPABASE_URL: process.env.SUPABASE_URL!,
} as const;
