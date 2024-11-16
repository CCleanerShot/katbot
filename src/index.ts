import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environments";
import { HypixelRoutes } from "./flipper/auction";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

async function test() {
	HypixelRoutes.getFinishedAuctions();
}

test();
