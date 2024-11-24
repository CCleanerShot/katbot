import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";
import { myUtils } from "./utils";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

const minutes = 60000;

// NOTE: dont run 2 async tasks cuz node has a fucking uncatchable error from an event
// what the fuck is thi shit

let counter = 0;
async function test() {
	counter++;

	if (counter > 2) {
		counter = 0;
		await hypixelController.GetGoodSales(bot.client);
		await myUtils.Sleep(minutes - 50000); // already taks a long ass time
	} else {
		await hypixelController.GetMoreData();
		await myUtils.Sleep(minutes);
	}

	test();
}

test();

process.on("uncaughtException", (err) => {
	console.log("uncaught", err);
});
