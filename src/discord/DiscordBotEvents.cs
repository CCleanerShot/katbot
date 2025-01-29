using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public partial class DiscordBot
{
    static void AddSignals()
    {
        _Client.InteractionCreated += InteractionCreated;
        _Client.Log += Log;
        _Client.Ready += Ready;
        _InteractionService.SlashCommandExecuted += SlashCommandExecuted;
    }

    static async Task InteractionCreated(SocketInteraction socketInteraction)
    {
        Utility.Log(Enums.LogLevel.NONE, $"{socketInteraction.User.GlobalName} used a command.");
        Utility.Log(Enums.LogLevel.NONE, $"{socketInteraction.Data}");

        SocketInteractionContext context = new SocketInteractionContext(_Client, socketInteraction);
        await _InteractionService.ExecuteCommandAsync(context, null);
    }

    static Task SlashCommandExecuted(SlashCommandInfo socketInteraction, IInteractionContext context, IResult result)
    {
        return Task.CompletedTask;
    }

    static Task Log(LogMessage message)
    {
        Utility.Log(Enums.LogLevel.NONE, message.ToString());
        return Task.CompletedTask;
    }

    static async Task Ready()
    {
        await _InteractionService.RegisterCommandsGloballyAsync();
        await _InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

        foreach (SocketGuild guild in _Client.Guilds)
            await _InteractionService.RegisterCommandsToGuildAsync(guild.Id);

        await _DiscordEvents.Load();
    }
}