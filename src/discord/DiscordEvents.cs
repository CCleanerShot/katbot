using System.Reflection;
using System.Timers;
using Discord.WebSocket;

/// <summary>
/// For usage inside DiscordEvent methods. NOTE: The method must be public!
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public partial class DiscordEventsAttribute : Attribute { }

/// <summary>
/// Create events in the /events folder.
/// </summary>
public partial class DiscordEvents
{
    /// <summary>
    /// The timer for the next time Hypixel API is checked.
    /// </summary>
    public readonly System.Timers.Timer _Timer = new System.Timers.Timer();
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

        _Timer.AutoReset = true;
        _Timer.Interval = Settings.PUBLIC_HYPIXEL_TIMER_MINUTES * 60000;
        _Timer.Start();

        return Task.CompletedTask;
    }

    public DiscordEvents(DiscordSocketClient _Client)
    {
        this._Client = _Client;
    }
}

