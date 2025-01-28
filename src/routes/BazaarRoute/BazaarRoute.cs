using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public partial class BazaarRoute : Route
{
    public double lastUpdated;
    public Dictionary<string, BazaarRouteProduct> products;

    public BazaarRoute(double _lastUpdated, Dictionary<string, BazaarRouteProduct> _products)
    {
        lastUpdated = _lastUpdated;
        products = _products;
    }

    public static async Task GetRoute(HttpClient client)
    {
        string baseURL = "https://api.hypixel.net/v2/skyblock";
        string json = await client.GetStringAsync($"{baseURL}/bazaar");
        JsonNode? result = JsonArray.Parse(json);

        if (result == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should not be null.");
            return;
        }

        if (result["success"] == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should always contains a 'success' field.");
            return;
        }

        if (result["success"]!.Equals(false))
        {
            Utility.Log(Enums.LogLevel.WARN, "Success = false, returning...");
            return;
        }

        BazaarRoute? bazaarRoute = JsonConvert.DeserializeObject<BazaarRoute>(json);

        if (bazaarRoute == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
            return;
        }

        Console.WriteLine(bazaarRoute.products);

        foreach (var (key, value) in bazaarRoute.products)
            if (Regex.IsMatch(key, @"\d"))
                Console.WriteLine(key);
    }
}