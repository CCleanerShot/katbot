using System.Reflection;
using System.Timers;
using Discord.WebSocket;

/// <summary>
/// For usage inside DiscordEvents methods. NOTE: The method must be public!
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public partial class DiscordEventsAttribute : Attribute { }

/// <summary>
/// Create events in the /events folder.
/// </summary>
public partial class DiscordEvents
{
    /// <summary>
    /// The timer for the next time Hypixel API is checked for auction items.
    /// </summary>
    public readonly System.Timers.Timer _AuctionTimer = new System.Timers.Timer();
    /// <summary>
    /// The timer for the next time Hypixel API is checked for bazaar items.
    /// </summary>
    public readonly System.Timers.Timer _BazaarTimer = new System.Timers.Timer();
    /// <summary>
    /// Client for the discord bot.
    /// </summary>
    DiscordSocketClient _Client;

    public Task Load()
    {
        IEnumerable<MethodInfo> methods = GetType().GetMethods()
            .Where(s => s.GetCustomAttribute<DiscordEventsAttribute>() != null);

        foreach (MethodInfo method in methods)
            method.Invoke(this, null);

        _AuctionTimer.AutoReset = true;
        _AuctionTimer.Interval = Settings.PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES * 60000;
        _AuctionTimer.Start();

        _BazaarTimer.AutoReset = true;
        _BazaarTimer.Interval = Settings.PUBLIC_HYPIXEL_BAZAAR_TIMER_MINUTES * 60000;
        _BazaarTimer.Start();

        return Task.CompletedTask;
    }

    public DiscordEvents(DiscordSocketClient _Client)
    {
        this._Client = _Client;
    }
}

