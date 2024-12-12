import { DiscordBot } from "./discord/client";
import { BotEnvironment } from "./environment";
import { hypixelController } from "./flipper/auction";
import { myUtils } from "./utils";
import { mongoClient } from "./custom/CustomMongoDB";

const bot = new DiscordBot();
bot.client.login(BotEnvironment.DISCORD_TOKEN);

const minutes = 60000;

// NOTE: dont run 2 async tasks cuz node has a fucking uncatchable error from an event

let counter = 0;
async function test() {
	counter++;

	await hypixelController.GetMoreData();
	await myUtils.Sleep(minutes);
	test();
}

async function run() {
	// connect to mongoDB
	await mongoClient.connect();

	test();
}

run();

process.on("uncaughtException", (err) => {
	console.log("uncaught", err);
});
