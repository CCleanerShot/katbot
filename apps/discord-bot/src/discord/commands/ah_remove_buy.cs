using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_remove_buy", "removes an auction item from your list")]
    public async Task ah_remove_buy(
        [Summary("item", "the item to remove from tracking"), fuckulinus(typeof(UserAuctionBuysAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.AuctionBuy.DeleteOneAsync(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString());
            await RespondAsync($"{MongoBot.CachedAuctionItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedAuctionBuys.Remove(MongoBot.CachedAuctionBuys.Find(e => e.ID == itemID && e.UserId == Context.User.Id)!);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}