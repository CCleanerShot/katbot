import { AttachmentBuilder, CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";
import { readFileSync } from "node:fs";

const buffer = readFileSync("images/brazil.gif");
module.exports = {
	data: new SlashCommandBuilder().setName("carlos").setDescription("brazillian goes to the store with $1"),

	async execute(interaction: CommandInteraction) {
		await interaction.reply({ files: [new AttachmentBuilder(buffer, { name: "brazil.gif", description: "brazillian goes to the store with $1" })] });
	},
};
