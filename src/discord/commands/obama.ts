import { AttachmentBuilder, CommandInteraction } from "discord.js";
import { SlashCommandBuilder } from "discord.js";
import { readFileSync } from "node:fs";

const buffer = readFileSync("images/obamacomputer.jpg");
module.exports = {
	data: new SlashCommandBuilder().setName("obama").setDescription("obama"),

	async execute(interaction: CommandInteraction) {
		await interaction.reply({ files: [new AttachmentBuilder(buffer, { name: "obamacomputer.png", description: "obama plays osu!" })] });
	},
};
