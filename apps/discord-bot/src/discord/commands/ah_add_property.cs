using Discord;
using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_add_property", "adds an property to an auction item")]
    public async Task ah_add_property(
    [Summary("item", "the item to add a property to"), Autocomplete(typeof(UserAuctionBuysAutocomplete))] string itemID,
    [Summary("property", "the property to check for"), Autocomplete(typeof(AuctionTagsAutocomplete))] string property,
    [Summary("value", "NOTE: if there is no autocomplete, input your value manually!"), Autocomplete(typeof(AuctionValueAutocomplete))] string value
    )
    {
        try
        {
            AuctionTag attribute = new AuctionTag(property, MongoBot.CachedAuctionTags[property].Type, value);
            UpdateDefinition<AuctionBuy> update = new UpdateDefinitionBuilder<AuctionBuy>()
                .Push(e => e.AuctionTags, attribute);

            await MongoBot.AuctionBuy.UpdateOneAsync(e => e.ID == itemID && e.UserId == Context.User.Id, update);
            MongoBot.CachedAuctionBuys.Find(e => e.ID == itemID && e.UserId == Context.User.Id)!.AuctionTags.Add(attribute);
            await RespondAsync($"Successfully added '{property}' to {MongoBot.CachedAuctionItems[itemID].Name} when the property is {value}.");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}