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


    public static async Task<AuctionsRouteProduct[]?> GetRoute()
    {
        try
        {
            string json = "";
            int maxRetry = 3;
            int currentRetry = 0;

            async Task setJson() => json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/auctions");

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



            return auctionsRoute.auctions;
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            return null;
        }
    }
}