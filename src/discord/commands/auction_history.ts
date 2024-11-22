import { CommandInteraction, TextChannel } from "discord.js";
import { supabaseClient } from "../../supabase/client";

const { SlashCommandBuilder } = require("discord.js");

module.exports = {
	data: new SlashCommandBuilder().setName("auction_history").setDescription("Fetches auction data"),

	async execute(interaction: CommandInteraction) {
		const results = await supabaseClient.client.from("auction_prices").select("*, auction_items (*) ");

		if (results.error) {
			await interaction.reply("Failed to fetch data, sorry!");
			return;
		}

		let stringBuilder = "";

		for (const item of results.data) {
			stringBuilder += item.auction_items?.name;
			stringBuilder += " | ";
			stringBuilder += item.average_price;
			stringBuilder += " | ";
			stringBuilder += item.total_sold;
			stringBuilder += "\n";
		}

		const chunks = stringBuilder.match(/[\s\S]{1,2000}/g) || [];
		const channel = (await interaction.client.channels.fetch(interaction.channelId)) as TextChannel;

		for (const chunk of chunks) {
			await channel.send(chunk);
		}

		await interaction.reply("Done!");
	},
};
