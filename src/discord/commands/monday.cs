using System.Reflection;
using Discord;
using Discord.Interactions;
using MongoDB.Driver;

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
            int chance = Program.Utility.NextRange(1, 3);

            string path = chance != 1 ? Settings.PUBLIC_PATH_MONDAY_GIF_URL : Settings.PUBLIC_PATH_MONDAY_GIF_URL_2;

            Embed embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithTitle("ramojusd — Yesterday at 23:34 omg midaro monsaaty")
                .WithImageUrl(path)
                .Build();

            await RespondAsync("", [embed]);
        }
    }
}