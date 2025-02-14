using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_remove_buy", "removes a bazaar item from the watchlist")]
    public async Task bz_remove_buy(
        [Summary("item", "the item to remove from tracking (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(UserBuysAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.BazaarBuy.DeleteOneAsync(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString());
            await RespondAsync($"{MongoBot.CachedBazaarItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedBazaarBuys.Remove(MongoBot.CachedBazaarBuys.Find(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString())!);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}