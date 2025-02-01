using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("remove_buy_item", "removes a bazaar item from the watchlist")]
    public async Task remove_buy_item(
        [Summary("item", "the item to remove from tracking (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(UserBuysAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.BazaarBuy.DeleteOneAsync(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString());
            await RespondAsync($"{MongoBot.CachedItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedBuys.Remove(MongoBot.CachedBuys.Find(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString())!);
        }

        catch (Exception)
        {
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}