using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

public partial class DiscordBot
{
    private static DiscordSocketClient _Client = default!;
    private static InteractionService _InteractionService = default!;
    private static DiscordEvents _DiscordEvents = default!;
    public static async Task Initialize()
    {
        DiscordSocketConfig discordConfig = new DiscordSocketConfig() { GatewayIntents = GatewayIntents.All };
        InteractionServiceConfig interactionConfig = new InteractionServiceConfig();
        interactionConfig.AutoServiceScopes = true;

        _Client = new DiscordSocketClient(discordConfig);
        _InteractionService = new InteractionService(_Client.Rest, interactionConfig);
        _DiscordEvents = new DiscordEvents(_Client);

        AddSignals();

        await _Client.LoginAsync(Discord.TokenType.Bot, Settings.DISCORD_TOKEN);
        await _Client.StartAsync();
    }
}