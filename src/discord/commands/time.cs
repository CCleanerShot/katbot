using System.Reflection;
using Discord;
using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("time", "its time to get a watch")]
    public async Task time()
    {
        string time = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm");

        await RespondAsync($"It is `{time}`.");
    }
}