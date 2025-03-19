using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_force", "forces the timer for checking the bazaar to finish instantly (same as '/bz_force')")]
    public async Task ah_force()
    {
        void OneTime(object? obj, System.Timers.ElapsedEventArgs args)
        {
            DiscordBot._DiscordEvents._AuctionTimer.Interval = Settings.PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES * 60000;
            DiscordBot._DiscordEvents._AuctionTimer.Elapsed -= OneTime;
        }

        try
        {
            DiscordBot._DiscordEvents._AuctionTimer.Interval = 100;
            DiscordBot._DiscordEvents._AuctionTimer.Elapsed += OneTime;
            await RespondAsync("Forcing (this will take a bit)...");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}