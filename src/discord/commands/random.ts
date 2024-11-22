import { CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";

module.exports = {
	data: new SlashCommandBuilder().setName("random").setDescription("journey to 727"),

	async execute(interaction: CommandInteraction) {
		const result = Math.round(Math.random() * 1000);
		await interaction.reply(result.toString());
	},
};
