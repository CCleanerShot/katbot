import { CommandInteraction, SlashCommandNumberOption } from "discord.js";
import { SlashCommandBuilder } from "discord.js";
import { BotEnvironment } from "../../environment";
import { myConfig } from "../../config";

module.exports = {
	data: new SlashCommandBuilder()
		.setName("config_minimum_price_for_sale")
		.setDescription("ADMIN COMMAND ONLY: updates config")
		.addNumberOption((option) =>
			option
				.setRequired(true)
				.setDescription("how long the difference is before its considered a sale")
				.setMinValue(0)
				.setName("minimum_price_for_sale"),
		),

	async execute(interaction: CommandInteraction) {
		const userID = interaction.user.id;

		if (userID !== BotEnvironment.ADMIN_1 && userID !== BotEnvironment.ADMIN_2) {
			await interaction.reply("fuck off");
			return;
		}

		const newValue = interaction.options.get("minimum_price_for_sale", true);

		if (typeof newValue.value !== "number") {
			await interaction.reply("how did u not give a number when you can only pass a number");
			return;
		}

		myConfig.data.MINIMUM_PRICE_FOR_SALE = newValue.value;
		myConfig.SaveConfig();
		await interaction.reply("Done!");
	},
};
