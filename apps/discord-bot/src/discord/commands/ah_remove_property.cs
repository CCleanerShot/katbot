using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_remove_property", "removes a property from an auction item")]
    public async Task ah_remove_property(
        [Summary("item", "the item to remove a property from"), Autocomplete(typeof(UserAuctionBuysAutocomplete))] string itemID,
        [Summary("property", "the property in question"), Autocomplete(typeof(UserAuctionBuyTagsAutocomplete))] string property,
        [Summary("value", "the value to remove"), Autocomplete(typeof(UserAuctionBuyTagsValueAutocomplete))] string value
    )
    {
        try
        {
            AuctionTag attribute = MongoBot.CachedAuctionBuys
                .Find(e => e.ID == itemID && e.UserId == Context.User.Id)!.AuctionTags
                .Find(e => e.Name == property)!;
            UpdateDefinition<AuctionBuy> update = new UpdateDefinitionBuilder<AuctionBuy>()
                .Pull(e => e.AuctionTags, attribute);

            await MongoBot.AuctionBuy.UpdateOneAsync(e => e.ID == itemID && e.UserId == Context.User.Id, update);
            MongoBot.CachedAuctionBuys.Find(e => e.ID == itemID && e.UserId == Context.User.Id)!.AuctionTags.Remove(attribute);
            await RespondAsync($"Successfully removed '{property}' to {MongoBot.CachedAuctionItems[itemID].Name} when the property is {value}.");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}