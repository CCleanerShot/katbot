package discordbot.discord;

public class DiscordBot {

}

// public class DiscordBot {
//     /**
//      * PROTECTED SET
//      */
//     public static DiscordClient _Client;
//     public static InteractionService _InteractionService;

//     public static void Initialize() {
//         _Client = DiscordClient.create("");
//         _InteractionService = new InteractionService(null);

//         _Client.gateway()
//                 .setEnabledIntents(IntentSet.all())
//                 .login()
//                 .block();
//     }
// }

//     public static async Task Initialize()
//     {
//         DiscordSocketConfig discordConfig = new DiscordSocketConfig() { GatewayIntents = GatewayIntents.All };
//         InteractionServiceConfig interactionConfig = new InteractionServiceConfig();
//         interactionConfig.AutoServiceScopes = true;

//         _Client = new DiscordSocketClient(discordConfig);
//         _InteractionService = new InteractionService(_Client.Rest, interactionConfig);
//         _DiscordEvents = new DiscordEvents(_Client);

//         AddSignals();

//         await _Client.LoginAsync(Discord.TokenType.Bot, Settings.DISCORD_TOKEN);
//         await _Client.StartAsync();
//     }