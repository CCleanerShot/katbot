public static partial class TimerBot
{
    /// <summary>
    /// List of tracked bazaar buys that have already been alerted.
    /// </summary>
    public static List<BazaarItem> WatchBuy_CachedBazaarBuyAlerts = new List<BazaarItem>();
    /// <summary>
    /// List of tracked bazaar sells that have already been alerted.
    /// </summary>
    public static List<BazaarItem> WatchSell_CachedBazaarSellAlerts = new List<BazaarItem>();

    static async void _BazaarElapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {
        Dictionary<string, BazaarRouteProduct>? liveItems = await BazaarRoute.GetRoute();

        if (liveItems == null)
            return;

        if (liveItems.Count == 0)
            return;

        List<BazaarItem> trackedBuys = await MongoBot.BazaarBuy.FindList(e => MongoBot.CachedBazaarItems.Keys.Contains(e.ID));
        List<BazaarItem> trackedSells = await MongoBot.BazaarSell.FindList(e => MongoBot.CachedBazaarItems.Keys.Contains(e.ID));

        List<BazaarItem> eligibleBuys = trackedBuys.Where(t =>
        {
            if (WatchBuy_CachedBazaarBuyAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            if (!liveItems.ContainsKey(t.ID)) // if the market somehow crashed for that item
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

        List<BazaarItem> eligibleSells = trackedSells.Where(t =>
        {
            if (WatchSell_CachedBazaarSellAlerts.Select(e => e.ID).Contains(t.ID))
                return false;

            if (!liveItems.ContainsKey(t.ID)) // if the market somehow crashed for that item
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

        Console.WriteLine($"{eligibleBuys.Count}, {eligibleSells.Count}");

        if (eligibleBuys.Count == 0 && eligibleSells.Count == 0)
            return;

        WebSocketBot.SendBazaarData(eligibleBuys, eligibleSells);
    }
}