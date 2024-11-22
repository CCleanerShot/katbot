import { CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";

module.exports = {
	data: new SlashCommandBuilder().setName("ramojusd").setDescription("ur mom"),

	async execute(interaction: CommandInteraction) {
		await interaction.reply("ur mom");
	},
};
