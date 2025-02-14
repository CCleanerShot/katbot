using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_add_buy", "adds an auction item to the watchlist for buying")]
    public async Task ah_add_buy(
        [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(BazaarItemAutocomplete))] string itemID,
        [Summary("attributes", "the list of attributes (or tags) to search for with the item")] AuctionTags[] tags,
        [Summary("buy_price", "the maximum buy price (alerts if lower)")] ulong buyPrice,
        [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            await RespondAsync("okay", null, false, true);
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}