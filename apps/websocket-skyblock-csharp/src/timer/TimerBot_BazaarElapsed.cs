public static partial class TimerBot
{
    static async void _BazaarElapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {
        Dictionary<string, BazaarRouteProduct>? liveItems = await BazaarRoute.GetRoute();

        if (liveItems == null)
            return;

        if (liveItems.Count == 0)
            return;

        List<BazaarItem> allBuys = await MongoBot.BazaarBuy.FindList(e => true);
        List<BazaarItem> allSells = await MongoBot.BazaarSell.FindList(e => true);

        // easier to just reset the current listings
        foreach (var (key, value) in MongoBot.EligibleBazaarBuys)
            MongoBot.EligibleBazaarBuys[key] = new List<BazaarItem>();
        foreach (var (key, value) in MongoBot.EligibleBazaarSells)
            MongoBot.EligibleBazaarSells[key] = new List<BazaarItem>();

        foreach (var (key, value) in MongoBot.EligibleBazaarBuys)
        {
            MongoBot.EligibleBazaarBuys[key] = allBuys.Where(t =>
            {
                if (t.UserId != key)
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
        }

        foreach (var (key, value) in MongoBot.EligibleBazaarSells)
        {
            MongoBot.EligibleBazaarSells[key] = allSells.Where(t =>
            {
                if (t.UserId != key)
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
        }

        WebSocketBot.SendBazaarData();
    }
}