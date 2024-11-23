import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";
import { myUtils } from "./utils";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

const minutes = 60000;
async function test() {
	await hypixelController.GetMoreData();
	await myUtils.Sleep(minutes * 2);
	test();
}

async function test2() {
	await hypixelController.GetGoodSales(bot.client);
	await myUtils.Sleep(minutes * 1);
	test2();
}

test();
test2();
