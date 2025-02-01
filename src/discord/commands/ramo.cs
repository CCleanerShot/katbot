using System.Reflection;
using Discord;
using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ramo", "ramojusd gameplay")]
    public async Task ramo()
    {
        Embed embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("brazillian goes to the store with $1")
            .WithImageUrl(Settings.PATH_RAMOJUSD_GIF_URL)
            .Build();

        await RespondAsync("", [embed]);
    }
}