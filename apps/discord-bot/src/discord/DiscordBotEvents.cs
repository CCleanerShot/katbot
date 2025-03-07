using System.Reflection;
using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;
using Discord.Rest;
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
        string name;

        if (socketInteraction is SocketAutocompleteInteraction socket1)
            name = socket1.Data.CommandName;
        else if (socketInteraction is SocketSlashCommand socket2)
            name = socket2.CommandName;
        else
            name = "N/A";

        Program.Utility.Log(Enums.LogLevel.NONE, $"{socketInteraction.User.GlobalName} used the command: '{name}'.");

        SocketInteractionContext context = new SocketInteractionContext(_Client, socketInteraction);
        await _InteractionService.ExecuteCommandAsync(context, null);
    }

    static Task _Log(LogMessage message)
    {
        string finalMessage = message.ToString();

        if (finalMessage.Contains("The remote party closed the WebSocket connection without completing the close handshake."))
        {
            Regex regex = new Regex(".+WebSocket connection was closed");
            finalMessage = regex.Match(finalMessage).Value;
        }

        Program.Utility.Log(Enums.LogLevel.NONE, finalMessage);
        return Task.CompletedTask;
    }

    static async Task _Ready()
    {
        await _InteractionService.AddCommandsGloballyAsync(true, []);
        await _InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

        foreach (SocketGuild guild in _Client.Guilds)
            await _InteractionService.RegisterCommandsToGuildAsync(guild.Id);

        await _DiscordEvents.Load();
    }
}