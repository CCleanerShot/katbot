using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("bz_add_sell", "adds a bazaar item to your list for selling")]
    public async Task bz_add_sell(
        [Summary("item", "the item to track"), Autocomplete(typeof(BazaarItemAutocomplete))] string itemID,
        [Summary("sell_price", "the maximum sell price (alerts if higher)")] ulong sellPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType,
        [Summary("remove_after", "whether or not to remove this item after it alerts the user")] bool removeAfter = true
    )
    {
        try
        {
            List<BazaarItem> response = await MongoBot.BazaarSell.FindList(e => e.ID == itemID && e.UserId == Context.User.Id);

            if (response.Count != 0)
            {
                BazaarItem item = response[0];
                item.Price = sellPrice;
                item.OrderType = orderType;
                item.RemovedAfter = removeAfter;
                await MongoBot.BazaarSell.ReplaceOneAsync(e => e._id == response[0]._id, item);
            }

            else
            {
                BazaarItem newItem = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedBazaarItems[itemID].Name,
                    Price = sellPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                    RemovedAfter = removeAfter,
                };

                await MongoBot.BazaarSell.InsertOneAsync(newItem);
                MongoBot.CachedBazaarSells.Add(newItem);
            }

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedBazaarItems[itemID].Name}**' if prices are **higher** than {sellPrice} ({orderType}).");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}