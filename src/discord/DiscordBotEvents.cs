using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public partial class DiscordBot
{
    static void AddSignals()
    {
        _Client.AutocompleteExecuted += AutocompleteExecuted;
        _Client.InteractionCreated += _InteractionCreated;
        _Client.Log += _Log;
        _Client.Ready += _Ready;

        // adding to stop the warnings that I'm not using them
        _Client.GuildScheduledEventCreated += (_) => Task.CompletedTask;
        _Client.InviteCreated += (_) => Task.CompletedTask;
        _Client.PresenceUpdated += (_, _, _) => Task.CompletedTask;
    }

    static async Task AutocompleteExecuted(SocketAutocompleteInteraction socketInteraction)
    {
        SocketInteractionContext context = new SocketInteractionContext(_Client, socketInteraction);
        await _InteractionService.ExecuteCommandAsync(context, null);
    }

    static async Task _InteractionCreated(SocketInteraction socketInteraction)
    {
        Utility.Log(Enums.LogLevel.NONE, $"{socketInteraction.User.GlobalName} used a command in {socketInteraction.Data}.");

        SocketInteractionContext context = new SocketInteractionContext(_Client, socketInteraction);
        await _InteractionService.ExecuteCommandAsync(context, null);
    }

    static Task _Log(LogMessage message)
    {
        Utility.Log(Enums.LogLevel.NONE, message.ToString());
        return Task.CompletedTask;
    }

    static async Task _Ready()
    {
        await _InteractionService.RegisterCommandsGloballyAsync();
        await _InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

        foreach (SocketGuild guild in _Client.Guilds)
            await _InteractionService.RegisterCommandsToGuildAsync(guild.Id);

        await _DiscordEvents.Load();
    }
}