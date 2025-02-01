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
            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price >= liveItems[t.ID].quick_status.buyPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price >= liveItems[t.ID].buy_summary.First().pricePerUnit)
                        return true;
                    else
                        return false;
                default:
                    throw new NotImplementedException("Implement this");
            }
        }).ToList();

        List<BazaarItem> elgibleSells = trackedSells.Where(t =>
        {
            switch (t.OrderType)
            {
                case Enums.OrderType.INSTA:
                    if (t.Price <= liveItems[t.ID].quick_status.sellPrice)
                        return true;
                    else
                        return false;
                case Enums.OrderType.ORDER:
                    if (t.Price <= liveItems[t.ID].sell_summary.First().pricePerUnit)
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
            Utility.Log(Enums.LogLevel.ERROR, "Attempted to send a message to the hypixel alerts channel, but it is missing!");
            return;
        }

        int maxName = "NAME".Length;
        int maxPrice = "CURRENT_PRICE".Length;
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
            response += $"**BUYS**\n";
            List<BazaarItem> buys = elgibleBuys.Where(e => e.UserId == k).ToList();
            response += $"{Utility.SS("NAME", maxName)}|{Utility.SS("CURRENT_PRICE", maxPrice)}|TRACKED PRICE\n";
            foreach (BazaarItem tracked in buys)
                response += $"{Utility.SS(tracked.Name, maxName)}|{Utility.SS(liveItems[tracked.ID].sell_summary.First().pricePerUnit.ToString(), maxPrice)}|{tracked.Price}\n";
            response += $"```\n";

            response += $"```\n";
            response += $"**SELLS**\n";
            List<BazaarItem> sells = elgibleSells.Where(e => e.UserId == k).ToList();
            response += $"{Utility.SS("NAME", maxName)}|{Utility.SS("CURRENT_PRICE", maxPrice)}|TRACKED PRICE\n";
            foreach (BazaarItem tracked in sells)
                response += $"{Utility.SS(tracked.Name, maxName)}|{Utility.SS(liveItems[tracked.ID].buy_summary.First().pricePerUnit.ToString(), maxPrice)}|{tracked.Price}\n";
            response += $"```\n";
        }

        await channel.SendMessageAsync(response);

        // caching items for the session, doing it after the discord message since it can fail
        foreach (BazaarItem tracked in elgibleBuys)
            WatchBuy_CachedBuyAlerts.Add(tracked);

        foreach (BazaarItem tracked in elgibleSells)
            WatchSell_CachedSellAlerts.Add(tracked);
    }
}