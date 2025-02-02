using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;

public partial class DiscordEvents
{
    /// <summary>
    /// List of tracked buys that have already been alerted.
    /// </summary>
    List<BazaarItem> WatchBuy_CachedBuyAlerts = new List<BazaarItem>();
    /// <summary>
    /// List of tracked sells that have already been alerted.
    /// </summary>
    List<BazaarItem> WatchSell_CachedSellAlerts = new List<BazaarItem>();

    [DiscordEvents]
    public void watch_bazaar()
    {
        _Timer.Elapsed += _Elapsed;
    }

    async void _Elapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {
        Dictionary<string, BazaarRouteProduct>? liveItems = await BazaarRoute.GetRoute();

        if (liveItems == null)
            return;

        List<BazaarItem> trackedBuys = (await MongoBot.BazaarBuy.FindAsync(e => MongoBot.CachedItems.Keys.Contains(e.ID))).ToList();
        List<BazaarItem> trackedSells = (await MongoBot.BazaarSell.FindAsync(e => MongoBot.CachedItems.Keys.Contains(e.ID))).ToList();

        List<BazaarItem> elgibleBuys = trackedBuys.Where(t =>
        {
            if (WatchBuy_CachedBuyAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price >= liveItems[t.ID].quick_status.buyPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price >= liveItems[t.ID].sell_summary.First().pricePerUnit)
                        return true;
                    else
                        return false;
                default:
                    throw new NotImplementedException("Implement this");
            }
        }).ToList();

        List<BazaarItem> elgibleSells = trackedSells.Where(t =>
        {
            if (WatchSell_CachedSellAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price <= liveItems[t.ID].quick_status.sellPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price <= liveItems[t.ID].buy_summary.First().pricePerUnit)
                        return true;
                    else
                        return false;
                default:
                    throw new NotImplementedException("Implement this");
            }
        }).ToList();


        if (elgibleSells.Count == 0 && elgibleSells.Count == 0)
            return;

        SocketTextChannel? channel = (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)) as SocketTextChannel;

        if (channel == null)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, "Attempted to send a message to the hypixel alerts channel, but it is missing!");
            return;
        }

        int maxName = "NAME".Length;
        int maxPrice = "LIVE_PRICE".Length;
        Dictionary<ulong, SocketGuildUser> cacheUsers = new Dictionary<ulong, SocketGuildUser>();

        // CHECKING MAX BUY STRING LENGTHS
        foreach (BazaarItem tracked in elgibleBuys)
        {
            if (maxName < tracked.Name.Length)
                maxName = tracked.Name.Length;
            if (maxPrice < liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString().Length)
                maxPrice = liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString().Length;

            SocketGuildUser user;
            // preventing spam to discord
            if (!cacheUsers.Keys.Contains(tracked.UserId))
            {
                user = (await (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)).GetUserAsync(tracked.UserId) as SocketGuildUser)!;
                cacheUsers.Add(user.Id, user);
            }
        }

        // CHECKING MAX SELL STRING LENGTHS
        foreach (BazaarItem tracked in elgibleSells)
        {
            if (maxName < tracked.Name.Length)
                maxName = tracked.Name.Length;
            if (maxPrice < liveItems[tracked.ID].buy_summary.First().pricePerUnit.ToString().Length)
                maxPrice = liveItems[tracked.ID].buy_summary.First().pricePerUnit.ToString().Length;

            SocketGuildUser user;
            // preventing spam to discord
            if (!cacheUsers.Keys.Contains(tracked.UserId))
            {
                user = (await (await DiscordBot._Client.GetChannelAsync(Settings.DISCORD_HYPIXEL_ALERTS_CHANNEL_ID)).GetUserAsync(tracked.UserId) as SocketGuildUser)!;
                cacheUsers.Add(user.Id, user);
            }
        }

        string response = "";

        // NOTE: contains unneeded o^2 notation, refactor if necessary.
        foreach (var (k, v) in cacheUsers)
        {
            response += $"<@{v.Id}>\n";
            response += $"```\n";

            response += $"{"**BUYS**"}\n";
            List<BazaarItem> buys = elgibleBuys.Where(e => e.UserId == k).ToList();
            response += $"{Program.Utility.SS("NAME", maxName)}|{Program.Utility.SS("LIVE_PRICE", maxPrice)}|WANTED_PRICE\n";
            foreach (BazaarItem tracked in buys)
            {
                string name = Program.Utility.SS(tracked.Name, maxName);
                string livePrice = Program.Utility.SS(liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString(), maxPrice);
                string wantedPrice = tracked.Price.ToString();
                response += $"{name}|{livePrice}|{wantedPrice}\n";
            }

            response += $"{"**SELLS**"}\n";
            List<BazaarItem> sells = elgibleSells.Where(e => e.UserId == k).ToList();
            response += $"{Program.Utility.SS("NAME", maxName)}|{Program.Utility.SS("LIVE_PRICE", maxPrice)}|WANTED_PRICE\n";
            foreach (BazaarItem tracked in sells)
            {

                string name = Program.Utility.SS(tracked.Name, maxName);
                string livePrice = Program.Utility.SS(liveItems[tracked.ID].buy_summary.First().pricePerUnit.ToString(), maxPrice);
                string wantedPrice = tracked.Price.ToString();
                response += $"{name}|{livePrice}|{wantedPrice}\n";
            }

            response += $"```\n";
        }

        await channel.SendMessageAsync(response);

        // caching items + removing after for those who can
        List<BazaarItem> toRemoveBuys = new List<BazaarItem>();
        List<BazaarItem> toRemoveSells = new List<BazaarItem>();

        foreach (BazaarItem tracked in elgibleBuys)
        {
            WatchBuy_CachedBuyAlerts.Add(tracked);

            if (tracked.RemovedAfter)
                toRemoveBuys.Add(tracked);
        }

        foreach (BazaarItem tracked in elgibleSells)
        {
            if (tracked.RemovedAfter)
                toRemoveSells.Add(tracked);

            WatchSell_CachedSellAlerts.Add(tracked);
        }

        // NOTE: o^2 notation, refactor if needed.
        await MongoBot.BazaarBuy.DeleteManyAsync(e => toRemoveBuys.Select(buy => buy.ID).Contains(e.ID));
        await MongoBot.BazaarSell.DeleteManyAsync(e => toRemoveSells.Select(sell => sell.ID).Contains(e.ID));
    }
}