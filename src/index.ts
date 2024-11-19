import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environments";
import { hypixelController } from "./flipper/auction";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

async function test() {
	await hypixelController.GetFinishedAuctions();
}

test();
