import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environments";

const bot = new DiscordBot();

bot.client.login(BotEnvironment.DISCORD_TOKEN);
