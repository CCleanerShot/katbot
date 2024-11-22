import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";
import { myUtils } from "./utils";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

/** for testing stuff right here */
async function test() {
	await hypixelController.GetMoreData();

	const minutes = 2;
	await myUtils.Sleep(60000 * minutes);
	test();
}

async function test2() {
	await hypixelController.GetGoodSales(bot.client);

	const minutes = 15;
	await myUtils.Sleep(60000 * minutes);
	test();
}

test();
test2();
