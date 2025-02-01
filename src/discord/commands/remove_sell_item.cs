using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("remove_sell_item", "removes a bazaar item from the watchlist")]
    public async Task remove_sell_item(
        [Summary("item", "the item to remove from tracking (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(UserSellsAutocomplete))] string itemID
    )
    {
        try
        {
            await MongoBot.BazaarSell.DeleteOneAsync(e => e.ID == itemID && e.Name == Context.User.Id.ToString());
            await RespondAsync($"{MongoBot.CachedItems[itemID].Name} removed from your watchlist!");
            MongoBot.CachedSells.Remove(MongoBot.CachedSells.Find(e => e.ID == itemID && e.Name == Context.User.Id.ToString())!);
        }

        catch (Exception)
        {
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}