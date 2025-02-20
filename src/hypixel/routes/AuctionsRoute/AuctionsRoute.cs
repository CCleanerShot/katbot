using System.Diagnostics;
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
    public static async Task<List<AuctionsRouteProduct>?> GetRoute(Dictionary<string, double?>? CachedItems = null, int pages = 100)
    {
        int i = 0;

        try
        {
            List<AuctionsRouteProduct> auctions = new List<AuctionsRouteProduct>();

            for (i = 0; i < pages; i++)
            {

                long proc1 = GC.GetTotalMemory(true);
                Program.Utility.Log(Enums.LogLevel.NONE, $"{proc1 / 1000000}MB (Total Start)", false, false);


                string json = "";
                int maxRetry = 3;
                int currentRetry = 0;

                async Task setJson() => json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/auctions?page={i}");

                try
                {
                    await setJson();
                }

                catch (Exception)
                {
                    currentRetry++;
                    Thread.Sleep(100);

                    Program.Utility.Log(Enums.LogLevel.WARN, $"Fetch failed! ({currentRetry})");
                    if (currentRetry >= maxRetry)
                        throw;

                    await setJson();
                }

                JsonNode? result = JsonArray.Parse(json);

                if (result == null)
                {
                    Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should not be null.");
                    return null;
                }

                if (result["success"] == null)
                {
                    Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should always contains a 'success' field.");
                    return null;
                }

                if (result["success"]!.Equals(false))
                {
                    Program.Utility.Log(Enums.LogLevel.WARN, "Success = false, returning...");
                    return null;
                }

                AuctionsRoute? auctionsRoute = JsonConvert.DeserializeObject<AuctionsRoute>(json);

                if (auctionsRoute == null)
                {
                    Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
                    return null;
                }

                foreach (AuctionsRouteProduct product in auctionsRoute.auctions)
                {
                    if (CachedItems != null)
                        if (!CachedItems.ContainsKey(product.uuid))
                            CachedItems.Add(product.uuid, default);
                        else
                            continue;

                    auctions.Add(product);
                }

                Program.Utility.Log(Enums.LogLevel.NONE, $"Page {i + 1} of auctions fetched.", true, false);

                if (auctionsRoute.auctions.Length < 1000)
                {
                    Program.Utility.Log(Enums.LogLevel.NONE, $"Found end of length at page {i + 1}. Closing...");
                    break;
                }

                await Task.Delay(10); // make sure we dont request too fast
            }

            return auctions;
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, $"Unexpected error on page {i}: {e}");
            return null;
        }
    }
}