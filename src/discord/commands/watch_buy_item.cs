using Discord.Interactions;
using MongoDB.Driver;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("watch_buy_item", "adds a bazaar item to the watchlist for buying")]
    public async Task watch_buy_item(
        [Summary("item", "the item to track (AUTOCOMPLETE => MAX 25)"), Autocomplete(typeof(ItemAutocomplete))] string itemID,
        [Summary("buy_price", "the maximum buy price (alerts if below)")] ulong buyPrice,
        [Summary("order_type", "the type of order it is")] Enums.OrderType orderType
    )
    {
        try
        {
            BazaarItem inserted;
            List<BazaarItem> response = (await MongoBot.BazaarBuy.FindAsync(e => e.ID == itemID && e.UserId == Context.User.Id)).ToList();

            if (response.Count != 0)
            {
                inserted = response[0];
                inserted.Price = buyPrice;
                inserted.OrderType = orderType;

                await MongoBot.BazaarBuy.ReplaceOneAsync(e => e._id == response[0]._id, inserted);
            }

            else
            {
                inserted = new BazaarItem()
                {
                    ID = itemID,
                    Name = MongoBot.CachedItems[itemID].Name,
                    Price = buyPrice,
                    OrderType = orderType,
                    UserId = Context.User.Id,
                };

                await MongoBot.BazaarBuy.InsertOneAsync(inserted);
            }

            if (MongoBot.CachedBuys.Find(e => e.ID == inserted.ID && e.Name == inserted.Name) == null)
                MongoBot.CachedBuys.Add(inserted);

            await RespondAsync($"OK! You will be alerted for '**{MongoBot.CachedItems[itemID].Name}**' if prices are **lower** than {buyPrice} ({orderType}).");
        }

        catch (Exception)
        {
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}