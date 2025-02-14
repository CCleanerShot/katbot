using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_remove_sell", "removes a bazaar item from your list")]
    public async Task bz_remove_sell(
        [Summary("item", "the item to remove from tracking (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(UserSellsAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.BazaarSell.DeleteOneAsync(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString());
            await RespondAsync($"{MongoBot.CachedBazaarItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedBazaarSells.Remove(MongoBot.CachedBazaarSells.Find(e => e.ID == itemID && e.UserId.ToString() == Context.User.Id.ToString())!);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}