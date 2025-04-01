using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class BazaarRoute
{
    public bool success = default!;
    public double lastUpdated = default!;
    public Dictionary<string, BazaarRouteProduct> products = new Dictionary<string, BazaarRouteProduct>();

    public static async Task<Dictionary<string, BazaarRouteProduct>?> GetRoute()
    {
        try
        {
            string json = "";
            int maxRetry = 3;
            int currentRetry = 0;

            async Task setJson() => json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/bazaar");

            try
            {
                await setJson();
            }

            catch (Exception)
            {
                currentRetry++;
                Thread.Sleep(100);

                Utility.Log(Enums.LogLevel.WARN, $"Fetch failed! ({currentRetry})");
                if (currentRetry >= maxRetry)
                    throw;

                await setJson();
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

            BazaarRoute? bazaarRoute = JsonConvert.DeserializeObject<BazaarRoute>(json);

            if (bazaarRoute == null)
            {
                Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
                return null;
            }

            return bazaarRoute.products;
        }

        catch (Exception e)
        {
            Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            return null;
        }
    }
}