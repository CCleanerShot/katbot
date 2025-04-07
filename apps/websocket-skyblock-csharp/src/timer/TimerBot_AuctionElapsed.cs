public static partial class TimerBot
{
    static async void _AuctionElapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute();

        if (liveItems == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Failed to fetch live items from the auction!");
            return;
        }

        List<AuctionBuy> allBuys = await MongoBot.AuctionBuy.FindList(e => true);
        List<AuctionsRouteProduct> similarLive = new List<AuctionsRouteProduct>();

        foreach (AuctionsRouteProduct item in liveItems)
            if (allBuys.Find(e => e.ID == item.ITEM_ID) != null)
                similarLive.Add(item);

        foreach (var (k, v) in MongoBot.ElgibleAuctionBuys)
            MongoBot.ElgibleAuctionBuys[k] = new List<AuctionItemsWithBuy>();

        foreach (var (k, v) in MongoBot.ElgibleAuctionBuys)
        {
            foreach (AuctionBuy trackedBuy in allBuys)
            {
                // items where the ID of an item from the API matches the same ID as a tracked item

                if (similarLive.Count == 0)
                    continue;

                // check if all the properties are matching
                foreach (AuctionsRouteProduct item in similarLive)
                {
                    foreach (AuctionTag attribute in item.AuctionTags)
                    {
                        if (trackedBuy.AuctionTags.All(attribute => item.AuctionTags.Any(e => e == attribute)))
                        {

                            if (!matchingProducts.ContainsKey(trackedBuy))
                                matchingProducts.Add(trackedBuy, new List<AuctionsRouteProduct>());

                            matchingProducts[trackedBuy].Add(item);
                            WatchBuy_CachedAuctionBuyAlerts[item.uuid].Add(trackedBuy.UserId);
                        }
                    }
                }
            }
        }

        Utility.LogPerformance("Finished Matching Products");
        WebSocketBot.SendAuctionData();
    }
}