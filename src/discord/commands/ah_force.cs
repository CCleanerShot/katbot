using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_force", "forces the timer for checking the bazaar to finish instantly (same as '/bz_force')")]
    public async Task ah_force()
    {
        try
        {
            DiscordBot._DiscordEvents._Timer.Interval = 100;
            DiscordBot._DiscordEvents._Timer.Elapsed += OneTime;
            await RespondAsync("Forcing...");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}