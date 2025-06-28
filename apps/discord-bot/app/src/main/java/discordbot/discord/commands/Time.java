package discordbot.discord.commands;

import net.dv8tion.jda.api.hooks.ListenerAdapter;
import discordbot.discord.DiscordCommands;
import net.dv8tion.jda.api.events.interaction.command.SlashCommandInteractionEvent;

// TODO: implement

@DiscordCommands(CommandName = "")
public class Time extends ListenerAdapter {
    @Override
    public void onSlashCommandInteraction(SlashCommandInteractionEvent event) {

    }
}

// using System.Reflection;
// using Discord;
// using Discord.Interactions;

// public partial class DiscordCommands : InteractionModuleBase
// {
//     [SlashCommand("time", "its time to get a watch")]
//     public async Task time()
//     {
//         int chance = Program.Utility.NextRange(1, 100);

//         if (chance == 1)
//         {
//             await RespondAsync("It's time for you to get a watch");
//         }

//         else
//         {
//             string time = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm");
//             await RespondAsync($"It is `{time}`.");
//         }
//     }
// }