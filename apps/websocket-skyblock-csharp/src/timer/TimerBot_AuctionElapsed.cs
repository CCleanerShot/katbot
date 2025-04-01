public static partial class TimerBot
{
    /// <summary>
    /// List of tracked auction IDs + list of discord IDs (users) that have already been alerted for.
    /// </summary>
    public static Dictionary<string, List<ulong>> WatchBuy_CachedAuctionBuyAlerts = new Dictionary<string, List<ulong>>();

    static async void _AuctionElapsed(object? obj, System.Timers.ElapsedEventArgs args)
    {

        List<AuctionsRouteProduct>? liveItems = await AuctionsRoute.GetRoute(WatchBuy_CachedAuctionBuyAlerts);

        if (liveItems == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Failed to fetch live items from the auction!");
            return;
        }

        List<AuctionBuy> eligibleBuys = await MongoBot.AuctionBuy.FindList(e => MongoBot.CachedAuctionBuys.Any(ee => ee.ID == e.ID));
        Dictionary<AuctionBuy, List<AuctionsRouteProduct>> matchingProducts = new Dictionary<AuctionBuy, List<AuctionsRouteProduct>>();

        foreach (AuctionBuy trackedBuy in eligibleBuys)
        {
            // items where the ID of an item from the API matches the same ID as a tracked item
            List<AuctionsRouteProduct> similarLive = liveItems.Where(e => e.ITEM_ID == trackedBuy.ID).ToList();

            if (similarLive.Count == 0)
                continue;

            // check if all the properties are matching
            foreach (AuctionsRouteProduct item in similarLive)
            {
                if (WatchBuy_CachedAuctionBuyAlerts.ContainsKey(item.uuid))
                    if (WatchBuy_CachedAuctionBuyAlerts[item.uuid].Contains(trackedBuy.UserId))
                        continue;

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

        Utility.LogPerformance("Finished Matching Products");

        if (matchingProducts.Count == 0)
            return;

        WebSocketBot.SendAuctionData(matchingProducts);
    }
}