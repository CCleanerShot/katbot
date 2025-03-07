using Discord;
using Discord.Interactions;
using Discord.WebSocket;

/// <summary>
/// Create commands inside the /commands folder.
/// </summary>
public partial class DiscordCommands : InteractionModuleBase
{
    void OneTime(object? obj, System.Timers.ElapsedEventArgs args)
    {
        DiscordBot._DiscordEvents._Timer.Interval = Settings.PUBLIC_HYPIXEL_TIMER_MINUTES * 60000;
        DiscordBot._DiscordEvents._Timer.Elapsed -= OneTime;
    }
}