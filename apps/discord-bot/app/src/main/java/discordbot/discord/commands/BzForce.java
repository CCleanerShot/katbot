package discordbot.discord.commands;

// TODO: implement

public class BzForce {

}

// using Discord.Interactions;
// using MongoDB.Driver;

// public partial class DiscordCommands : InteractionModuleBase
// {
//     [SlashCommand("bz_force", "forces the timer for checking the bazaar to finish instantly (same as '/ah_force')")]
//     public async Task bz_force()
//     {
//         void OneTime(object? obj, System.Timers.ElapsedEventArgs args)
//         {
//             DiscordBot._DiscordEvents._BazaarTimer.Interval = Settings.PUBLIC_HYPIXEL_BAZAAR_TIMER_MINUTES * 60000;
//             DiscordBot._DiscordEvents._BazaarTimer.Elapsed -= OneTime;
//         }

//         try
//         {
//             DiscordBot._DiscordEvents._BazaarTimer.Interval = 100;
//             DiscordBot._DiscordEvents._BazaarTimer.Elapsed += OneTime;
//             await RespondAsync("Forcing...");
//         }

//         catch (Exception e)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
//             await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
//         }
//     }
// }