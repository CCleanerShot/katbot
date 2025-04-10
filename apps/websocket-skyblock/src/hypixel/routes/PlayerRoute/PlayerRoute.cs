using System.Text.Json.Nodes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ZstdSharp.Unsafe;

public class PlayerRoute
{
    public bool success = true;
    public PlayerRoutePlayer player = default!;

    /// <summary>
    /// NOTE: This returns a JsonNode because the data is very dynamic and not worth checking.
    /// </summary>
    /// <returns></returns>
    public static async Task<PlayerRoutePlayer?> GetRoute(string player_id)
    {
        string json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/player?uuid={player_id}");
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

        PlayerRoute? route = JsonConvert.DeserializeObject<PlayerRoute>(json);

        if (route == null)
        {
            Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
            return null;
        }


        return route.player;
    }
}
