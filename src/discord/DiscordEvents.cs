using System.Reflection;
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
    DiscordSocketClient _Client;

    public Task Load()
    {
        IEnumerable<MethodInfo> methods = GetType().GetMethods()
            .Where(s => s.GetCustomAttribute<DiscordEventsAttribute>() != null);

        foreach (MethodInfo method in methods)
            method.Invoke(this, null);

        return Task.CompletedTask;
    }

    public DiscordEvents(DiscordSocketClient _Client)
    {
        this._Client = _Client;
    }
}

