import { Client, Events, GatewayIntentBits, TextChannel } from "discord.js";

export const client = new Client({
	intents: [GatewayIntentBits.MessageContent, GatewayIntentBits.GuildMessages, GatewayIntentBits.GuildMessageTyping, GatewayIntentBits.GuildPresences],
});

client.once(Events.ClientReady, async () => {
	const guilds = await client.guilds.fetch();

	guilds.forEach(async (g) => {
		if (g.id == "530645640280277003") {
			const channel: TextChannel = (await client.channels.fetch("532127696751165441")) as TextChannel;
			await channel.send("ur mom");
		}
	});
});
