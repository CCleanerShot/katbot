import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

/** for testing stuff right here */
async function test() {
	await hypixelController.GetOngoingAuctions();
}

test();
