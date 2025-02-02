using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_force", "forces the timer for checking the bazaar to finish instantly")]
    public async Task bz_force()
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

    void OneTime(object? obj, System.Timers.ElapsedEventArgs args)
    {
        DiscordBot._DiscordEvents._Timer.Interval = Settings.HYPIXEL_TIMER_MINUTES * 60000;
        DiscordBot._DiscordEvents._Timer.Elapsed -= OneTime;
    }
}