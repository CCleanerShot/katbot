package discordbot.discord;

import java.util.EnumSet;
import discordbot.S;
import discordbot.common.Enums.Settings;
import net.dv8tion.jda.api.JDA;
import net.dv8tion.jda.api.JDABuilder;
import net.dv8tion.jda.api.requests.GatewayIntent;

public class DiscordBot {
    public static void Initialize() {
        JDA discordClient = JDABuilder.createDefault(S.Get(Settings.DISCORD_TOKEN))
                .enableIntents(EnumSet.allOf(GatewayIntent.class))
                .build();

        try {
            discordClient.awaitReady();
        } catch (InterruptedException ie) {

        }

    }
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