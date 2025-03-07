using System.Text.Json.Nodes;
using MongoDB.Bson;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

public class ItemsRoute
{
    /// <summary>
    /// NOTE: This returns a JsonNode because the data is very dynamic and not worth checking.
    /// </summary>
    /// <returns></returns>
    public static async Task<JsonNode?> GetRoute()
    {
        string json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/resources/skyblock/items");
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

        return result;
    }
}
