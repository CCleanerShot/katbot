using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_remove_sell", "removes a bazaar item from your list")]
    public async Task bz_remove_sell(
        [Summary("item", "the item to remove from tracking"), fuckulinus(typeof(UserBazaarSellsAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.BazaarSell.DeleteOneAsync(e => e.ID == itemID && e.UserId == Context.User.Id);
            await RespondAsync($"{MongoBot.CachedBazaarItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedBazaarSells.Remove(MongoBot.CachedBazaarSells.Find(e => e.ID == itemID && e.UserId == Context.User.Id)!);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}