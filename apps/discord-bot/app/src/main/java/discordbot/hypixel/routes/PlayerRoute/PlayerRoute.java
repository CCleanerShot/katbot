package discordbot.hypixel.routes.PlayerRoute;

import java.net.URI;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse.BodyHandlers;
import java.text.MessageFormat;

import discordbot.BotSettings;
import discordbot.Main;
import discordbot.common.Enums.LogLevel;
import discordbot.common.Enums.Settings;

public class PlayerRoute {
    public boolean success;
    public PlayerRoutePlayer player;

    /**
    * NOTE: This returns an Object because the data is very dynamic and not worth checking.
    */
    public static Object GetRoute(String player_id) {
        String json = new String();
        int maxRetry = 3;
        int currentRetry = 0;
        Object product = new Object();

        try {
            do {
                try {
                    URI uri = new URI(MessageFormat.format("{0}/skyblock/player?uuid={1}", BotSettings.Get(Settings.HYPIXEL_API_BASE_URL), player_id));
                    HttpRequest request = HttpRequest.newBuilder(uri).header("API-Key", BotSettings.Get(Settings.HYPIXEL_BOT_KEY)).build();
                    json = Main.Client.send(request, BodyHandlers.ofString()).body();
                    product = Main.Gson.fromJson(json, Object.class);
                    currentRetry = maxRetry + 1;
                } catch (Exception e) {
                    Thread.sleep(100);
                    currentRetry += 1;
                    Main.Utility.Log(LogLevel.WARN, MessageFormat.format("Fetch failed! ({0})", e));

                    if (currentRetry >= maxRetry)
                        throw new Exception(MessageFormat.format("Reached max retry limit! Throwing... {0}", e));
                }
            } while (currentRetry <= maxRetry);
        } catch (Exception err) {
            Main.Utility.Log(LogLevel.ERROR, MessageFormat.format("Unhandled Exception: {0}", err));
        }

        return product;
    }
}

//     /// <summary>
//     /// NOTE: This returns a JsonNode because the data is very dynamic and not worth checking.
//     /// </summary>
//     /// <returns></returns>
//     public static async Task<PlayerRoutePlayer?> GetRoute(string player_id)
//     {
//         string json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/player?uuid={player_id}");
//         JsonNode? result = JsonArray.Parse(json);

//         if (result == null)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should not be null.");
//             return null;
//         }

//         if (result["success"] == null)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should always contains a 'success' field.");
//             return null;
//         }

//         if (result["success"]!.Equals(false))
//         {
//             Program.Utility.Log(Enums.LogLevel.WARN, "Success = false, returning...");
//             return null;
//         }

//         PlayerRoute? route = JsonConvert.DeserializeObject<PlayerRoute>(json);

//         if (route == null)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
//             return null;
//         }

//         return route.player;
//     }
// }