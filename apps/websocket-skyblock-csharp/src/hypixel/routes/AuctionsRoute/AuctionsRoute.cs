using System.Text.Json.Nodes;
using Newtonsoft.Json;
using ZstdSharp.Unsafe;

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
            auctions = await BatchFetch(10);
        }
        else
        {
            auctions = await BatchFetch(100);
            MongoBot.AuctionBuysRecentlyUpdated = true;
        }

        if (auctions == null)
            return null;

        return auctions;
    }

    static async Task<List<AuctionsRouteProduct>?> BatchFetch(int pages)
    {
        List<AuctionsRouteProduct> products = new List<AuctionsRouteProduct>();
        int i = 0;

        try
        {
            AuctionsRoute? firstResults = await GetResult(0);

            if (firstResults == null)
                return null;

            // NOTE: im doing it this way instead of an array of tasks, because for some reason, an async function within a task has unexpected results when awaiting (skips it + the rest of the code)
            products.AddRange(firstResults.auctions);
            int totalPages = Math.Min(firstResults.totalAuctions / 1000, pages);
            Console.WriteLine(firstResults.totalAuctions);
            Action?[] tasks = new Action[totalPages - 1];
            Task dummyTask = new Task(() => { }); // used to mimic all tasks being done
            List<AuctionsRouteProduct>[] fetchedProducts = new List<AuctionsRouteProduct>[totalPages - 1];

            for (i = 1; i < totalPages; i++)
            {
                int page = i;
                Action task = async () =>
                {
                    Utility.Log(Enums.LogLevel.NONE, $"auction page {page}", false, false);
                    AuctionsRoute? auctionsRoute = await GetResult(page);

                    if (auctionsRoute == null)
                        return;

                    products.AddRange(auctionsRoute.auctions);
                    fetchedProducts[page - 1] = auctionsRoute.auctions.ToList();
                    tasks[page - 1] = null;

                    if (tasks.All(e => e == null))
                        dummyTask.Start();
                };

                tasks[page - 1] = task;
            }

            foreach (Action? task in tasks)
                task?.Invoke();

            Task.WaitAll(dummyTask);

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
            Thread.Sleep(500);

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