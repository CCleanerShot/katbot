using System.Reflection;
using Discord;
using Discord.Interactions;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("monday", "tuesday")]
    public async Task monday()
    {
        if (DateTime.UtcNow.DayOfWeek != DayOfWeek.Monday)
        {
            await RespondAsync("It's not Monday yet 😦.");
        }

        else
        {
            Embed embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithTitle("ramojusd — Yesterday at 23:34 omg midaro monsaaty")
                .WithImageUrl(Settings.PUBLIC_PATH_MONDAY_GIF_URL)
                .Build();

            await RespondAsync("", [embed]);
        }
    }
}