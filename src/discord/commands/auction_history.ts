import { myUtils } from "../../utils";
import { supabaseClient } from "../../supabase/client";
import { CommandInteraction, TextChannel } from "discord.js";
import { SlashCommandBuilder } from "discord.js";

module.exports = {
	data: new SlashCommandBuilder().setName("auction_history").setDescription("Fetches auction data"),

	async execute(interaction: CommandInteraction) {
		const results = await supabaseClient.client.from("auction_items").select("*, auction_items (*) ");

		if (results.error) {
			await interaction.reply("Failed to fetch data, sorry!");
			return;
		}

		let stringBuilder = "";

		for (const item of results.data) {
			stringBuilder += item.name;
			stringBuilder += " | ";
			stringBuilder += item.average_price;
			stringBuilder += " | ";
			stringBuilder += item.total_sold;
			stringBuilder += "\n";
		}

		const channel = (await interaction.client.channels.fetch(interaction.channelId)) as TextChannel;

		await myUtils.SendBulkText(channel, stringBuilder);
		await interaction.reply("Done!");
	},
};
