import { CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";
import { BotEnvironment } from "../../environment";
import { hypixelController } from "../../flipper/auction";

module.exports = {
	data: new SlashCommandBuilder()
		.setName("auction_save_data")
		.setDescription("ADMIN COMMAND ONLY: gets finished auctions and saves them"),

	async execute(interaction: CommandInteraction) {
		const userID = interaction.user.id;

		if (userID !== BotEnvironment.ADMIN_1 && userID !== BotEnvironment.ADMIN_2) {
			await interaction.reply("fuck off");
			return;
		}

		await hypixelController.GetMoreData();
		await interaction.reply("Done!");
	},
};
