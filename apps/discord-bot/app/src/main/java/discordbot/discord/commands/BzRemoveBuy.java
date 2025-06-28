package discordbot.discord.commands;

// TODO: implement

public class BzRemoveBuy {

}

// using Discord.Interactions;
// using MongoDB.Driver;

// public partial class DiscordCommands : InteractionModuleBase
// {
//     [SlashCommand("bz_remove_buy", "removes a bazaar item from your list")]
//     public async Task bz_remove_buy(
//         [Summary("item", "the item to remove from tracking"), Autocomplete(typeof(UserBazaarBuysAutocomplete))] string itemID
//     )
//     {
//         try
//         {
//             await MongoBot.BazaarBuy.DeleteOneAsync(e => e.ID == itemID && e.UserId == Context.User.Id);
//             await RespondAsync($"{MongoBot.CachedBazaarItems[itemID].Name} removed from your watchlist!");
//             MongoBot.CachedBazaarBuys.Remove(MongoBot.CachedBazaarBuys.Find(e => e.ID == itemID && e.UserId == Context.User.Id)!);
//         }

//         catch (Exception e)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
//             await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
//         }
//     }
// }