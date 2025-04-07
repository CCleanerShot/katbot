using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

public class AuctionsRoute
{
    public bool success;
    public int page;
    public int totalPages;
    public int totalAuctions;
    public double lastUpdated;
    public AuctionsRouteProduct[] auctions = default!;

    /// <summary>
    /// Note: The optional cache is meant to be used on all incoming items, hopefully making subsequent actions easier.
    /// </summary>
    /// <param name="CachedItems"></param>
    /// <returns></returns>
    public static async Task<List<AuctionsRouteProduct>?> GetRoute()
    {
        // there will be 2 fetch types:
        // > Default Fetch: fetches all pages, 5minute cooldown
        // > Max Fetch: fetches latest 5 pages, 1minute cooldown
        // 
        // Max Fetch will occur during the following:
        // - very start of the application launch
        // - when there is a new/modified item in the feed
        // 
        // otherwise, use the Default Fetch
        List<AuctionsRouteProduct>? auctions;

        if (MongoBot.AuctionBuysRecentlyUpdated)
        {
            auctions = await DefaultFetch();
        }
        else
        {
            auctions = await MaxFetch();
            MongoBot.AuctionBuysRecentlyUpdated = true;
        }

        if (auctions == null)
            return null;

        return auctions;
    }

    static async Task<List<AuctionsRouteProduct>?> DefaultFetch()
    {
        List<AuctionsRouteProduct> products = new List<AuctionsRouteProduct>();
        int i = 0;

        try
        {
            for (i = 0; i < 10; i++)
            {
                Utility.Log(Enums.LogLevel.NONE, $"auction page {i}", false, false);
                AuctionsRoute? auctionsRoute = await GetResult(i);

                if (auctionsRoute == null)
                    return null;

                products.AddRange(auctionsRoute.auctions);

                if (auctionsRoute.auctions.Length < 1000)
                {
                    Utility.Log(Enums.LogLevel.NONE, "Look into this: this means less than 10,000 items exist on the auction. Potential Event!");
                    return products;
                }
            }

            return products;
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, $"Unexpected error on page {i}: {e}");
            return null;
        }
    }

    static async Task<List<AuctionsRouteProduct>?> MaxFetch()
    {
        List<AuctionsRouteProduct> products = new List<AuctionsRouteProduct>();
        int i = 0;

        try
        {
            for (i = 0; i < 100; i++)
            {
                Utility.Log(Enums.LogLevel.NONE, $"auction page {i}", false, false);
                AuctionsRoute? auctionsRoute = await GetResult(i);

                if (auctionsRoute == null)
                    return null;

                products.AddRange(auctionsRoute.auctions);

                if (auctionsRoute.auctions.Length < 1000)
                    return products;
            }

            return products;
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, $"Unexpected error on page {i}: {e}");
            return null;
        }
    }

    static async Task<AuctionsRoute?> GetResult(int page)
    {
        string json;
        int maxRetry = 3;
        int currentRetry = 0;

        try
        {
            json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/auctions?page={page}");
        }

        catch (Exception)
        {
            currentRetry++;
            Utility.Log(Enums.LogLevel.WARN, $"Fetch failed! ({currentRetry})");
            Thread.Sleep(50);

            if (currentRetry >= maxRetry)
                return null;

            json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/auctions?page={page}");
        }

        JsonNode? result = JsonArray.Parse(json);

        if (result == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should not be null.");
            return null;
        }

        if (result["success"] == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should always contains a 'success' field.");
            return null;
        }

        if (result["success"]!.Equals(false))
        {
            Utility.Log(Enums.LogLevel.WARN, "Success = false, returning...");
            return null;
        }

        AuctionsRoute? auctionsRoute = JsonConvert.DeserializeObject<AuctionsRoute>(json);

        if (auctionsRoute == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
            return null;
        }

        return auctionsRoute;
    }
}