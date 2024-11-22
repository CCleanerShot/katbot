import { CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";
import { hypixelController } from "../../flipper/auction";

module.exports = {
	data: new SlashCommandBuilder().setName("auction_sales").setDescription("Fetches current auction sales"),

	async execute(interaction: CommandInteraction) {
		await interaction.reply("Hold up...");
		await hypixelController.GetGoodSales(interaction.client);
	},
};
