using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_add_buy", "adds a bazaar item to your list for buying")]
    public async Task bz_add_buy(
        [Summary("item", "the item to track"), Autocomplete(typeof(BazaarItemAutocomplete))] string itemID,
        [Summary("buy_price", "the maximum buy price (alerts if lower)")] ulong buyPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType,
        [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            List<BazaarItem> response = await MongoBot.BazaarBuy.FindList(e => e.ID == itemID && e.UserId == Context.User.Id);

            if (response.Count != 0)
            {
                BazaarItem item = response[0];
                item.Price = buyPrice;
                item.OrderType = orderType;
                item.RemovedAfter = removeAfter;
                await MongoBot.BazaarBuy.ReplaceOneAsync(e => e._id == response[0]._id, item);
            }

            else
            {
                BazaarItem newItem = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedBazaarItems[itemID].Name,
                    Price = buyPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                    RemovedAfter = removeAfter,
                };

                await MongoBot.BazaarBuy.InsertOneAsync(newItem);
                MongoBot.CachedBazaarBuys.Add(newItem);
            }

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedBazaarItems[itemID].Name}**' if prices are **lower** than {buyPrice} ({orderType}).");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}