using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class BazaarRoute
{
    public bool success;
    public double lastUpdated;
    /// <summary>
    /// NOTE: Do not use! Used for deserialization from response.
    /// </summary>
    public readonly Dictionary<string, BazaarRouteProduct> products = new Dictionary<string, BazaarRouteProduct>();

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

            BazaarRoute? bazaarRoute = JsonConvert.DeserializeObject<BazaarRoute>(json);

            if (bazaarRoute == null)
            {
                Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
                return null;
            }

            Dictionary<string, BazaarRouteProduct> products = new Dictionary<string, BazaarRouteProduct>();

            foreach (var (k, v) in bazaarRoute.products)
                products.Add(k, v);

            return products;
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            return null;
        }
    }
}