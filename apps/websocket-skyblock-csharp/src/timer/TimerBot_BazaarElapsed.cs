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
            MongoBot.EligibleBazaarBuys[key] = new List<BazaarSocketMessage>();
        foreach (var (key, value) in MongoBot.EligibleBazaarSells)
            MongoBot.EligibleBazaarSells[key] = new List<BazaarSocketMessage>();

        foreach (var (key, value) in MongoBot.EligibleBazaarBuys)
        {
            foreach (BazaarItem item in allBuys)
            {
                if (item.UserId != key)
                    continue;

                if (!liveItems.ContainsKey(item.ID)) // if the market somehow crashed for that item
                    continue;

                BazaarRouteProduct liveItem = liveItems[item.ID];

                switch (item.OrderType)
                {
                    case Enums.OrderType.INSTA:
                        if (item.Price >= liveItems[item.ID].quick_status.buyPrice)
                            MongoBot.EligibleBazaarBuys[key].Add(new BazaarSocketMessage(liveItem, item));

                        continue;
                    case Enums.OrderType.ORDER:
                        if (item.Price >= liveItems[item.ID].sell_summary.First().pricePerUnit)
                            MongoBot.EligibleBazaarBuys[key].Add(new BazaarSocketMessage(liveItem, item));

                        continue;
                }
            }
        }

        foreach (var (key, value) in MongoBot.EligibleBazaarSells)
        {
            foreach (BazaarItem item in allSells)
            {
                if (item.UserId != key)
                    continue;

                if (!liveItems.ContainsKey(item.ID)) // if the market somehow crashed for that item
                    continue;

                BazaarRouteProduct liveItem = liveItems[item.ID];

                switch (item.OrderType)
                {
                    case Enums.OrderType.INSTA:
                        if (item.Price <= liveItems[item.ID].quick_status.sellPrice)
                            MongoBot.EligibleBazaarSells[key].Add(new BazaarSocketMessage(liveItem, item));

                        continue;
                    case Enums.OrderType.ORDER:
                        if (item.Price <= liveItems[item.ID].buy_summary.First().pricePerUnit)
                            MongoBot.EligibleBazaarSells[key].Add(new BazaarSocketMessage(liveItem, item));

                        continue;
                }
            }
        }

        WebSocketBot.SendBazaarData();
    }
}