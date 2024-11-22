import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);
