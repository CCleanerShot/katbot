using Discord;
using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("ah_add_buy", "adds an auction item to your list for buying. NOTE: /ah_add_property to add properties")]
    public async Task ah_add_buy(
    [Summary("item", "the item to track"), fuckulinus(typeof(AuctionItemAutocomplete))] string itemID,
    [Summary("buy_price", "the minimum buy price (alerts if lower)")] ulong buyPrice,
    [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            List<AuctionBuy> response = await MongoBot.AuctionBuy.FindList(e => e.ID == itemID && e.UserId == Context.User.Id);

            if (response.Count != 0)
            {
                AuctionBuy existingItem = response[0];
                existingItem.Price = buyPrice;
                existingItem.RemovedAfter = removeAfter;
                await MongoBot.AuctionBuy.ReplaceOneAsync(e => e.ID == itemID && e.UserId == Context.User.Id, existingItem);
            }

            else
            {
                AuctionBuy newItem = new AuctionBuy()
                {
                    ID = itemID,
                    AuctionTags = new List<AuctionTag>(),
                    Name = MongoBot.CachedAuctionItems[itemID].Name,
                    Price = buyPrice,
                    RemovedAfter = removeAfter,
                    UserId = Context.User.Id,
                };

                await MongoBot.AuctionBuy.InsertOneAsync(newItem);
                MongoBot.CachedAuctionBuys.Add(newItem);
            }

            await RespondAsync($"Started watching the '{MongoBot.CachedAuctionItems[itemID].Name}'!\nYou can add properties to it with the command, `/ah_add_property`.");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}