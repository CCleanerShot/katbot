import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";
import { myUtils } from "./utils";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

/** for testing stuff right here */
async function test() {
	await hypixelController.GetMoreData();

	const minutes = 5;
	await myUtils.Sleep(60000 * minutes);
	test();
}

test();
