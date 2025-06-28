package discordbot.discord.commands;

// TODO: implement

public class Monday {

}

// using System.Reflection;
// using Discord;
// using Discord.Interactions;
// using MongoDB.Driver;

// public partial class DiscordCommands : InteractionModuleBase
// {
//     [SlashCommand("monday", "tuesday")]
//     public async Task monday()
//     {
//         if (DateTime.UtcNow.DayOfWeek != DayOfWeek.Sunday && DateTime.UtcNow.DayOfWeek != DayOfWeek.Monday && DateTime.UtcNow.DayOfWeek != DayOfWeek.Tuesday)
//         {
//             await RespondAsync("It's not Monday yet 😦.");
//         }

//         else
//         {
//             int chance = Program.Utility.NextRange(1, 3);

//             string title;
//             string path;

//             if (chance == 1)
//             {
//                 title = "tommorow is wednesday";
//                 path = Settings.PUBLIC_PATH_MONDAY_GIF_URL_2;
//             }

//             else
//             {
//                 title = "ramojusd — Yesterday at 23:34 omg midaro monsaaty";
//                 path = Settings.PUBLIC_PATH_MONDAY_GIF_URL;
//             }

//             Embed embed = new EmbedBuilder()
//                 .WithColor(Color.Red)
//                 .WithTitle(title)
//                 .WithImageUrl(path)
//                 .Build();

//             await RespondAsync("", [embed]);
//         }
//     }
// }