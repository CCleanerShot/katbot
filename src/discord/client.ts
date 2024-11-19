import fs from "fs";
import path from "node:path";
import { Client, Collection, Events, GatewayIntentBits, REST, Routes, TextChannel } from "discord.js";
import { BotEnvironment } from "../environment";
import { utils } from "../utils";

export class DiscordBot {
	client: Client;
	commands: Collection<string, any>;
	constructor() {
		this.client = new Client({ intents: [GatewayIntentBits.MessageContent, GatewayIntentBits.GuildMessages, GatewayIntentBits.GuildMessageTyping, GatewayIntentBits.GuildPresences] });
		this.commands = new Collection();
		this.client.once(Events.ClientReady, async () => {
			const guilds = await this.client.guilds.fetch();

			guilds.forEach(async (g) => {
				if (g.id == "530645640280277003") {
					const channel: TextChannel = (await this.client.channels.fetch("532127696751165441")) as TextChannel;
					// await channel.send("ramojusd");
				}
			});

			const foldersPath = path.join(__dirname, "commands");
			const commandFiles = fs.readdirSync(foldersPath).filter((file) => file.endsWith(".js"));

			const resultCommands: any[] = [];
			for (const file of commandFiles) {
				const filePath = path.join(foldersPath, file);
				const command = require(filePath);
				if ("data" in command) {
					this.commands.set(command.data.name, command);
					resultCommands.push(command.data.toJSON());
				} else {
					console.log(`[WARNING] The command at ${filePath} is missing a required "data" or "execute" property.`);
				}
			}

			const rest = new REST().setToken(BotEnvironment.DISCORD_TOKEN);

			const addCommands = async () => {
				for (const [string, guild] of await this.client.guilds.fetch()) {
					if (guild.id == BotEnvironment.DISCORD_MAIN_CHANNEL_ID) {
						const data = await rest.put(Routes.applicationGuildCommands(BotEnvironment.DISCORD_APPLICATION_ID, guild.id), { body: resultCommands });
						await utils.Sleep(100);
					}
				}
			};

			addCommands();

			this.client.on(Events.InteractionCreate, async (interaction) => {
				if (!interaction.isChatInputCommand()) return;

				const command = await this.commands.get(interaction.commandName);

				if (!command) {
					console.error(`No command matching ${interaction.commandName} was found.`);
					return;
				}

				try {
					await command.execute(interaction);
				} catch (error) {
					console.error(error);
					if (interaction.replied || interaction.deferred) {
						await interaction.followUp({ content: "There was an error while executing this command!", ephemeral: true });
					} else {
						await interaction.reply({ content: "There was an error while executing this command!", ephemeral: true });
					}
				}
			});
		});
	}
}
