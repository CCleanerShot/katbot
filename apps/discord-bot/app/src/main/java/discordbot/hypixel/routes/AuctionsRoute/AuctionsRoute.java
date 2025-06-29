package discordbot.hypixel.routes.AuctionsRoute;

import com.google.gson.Gson;
import java.net.URI;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse.BodyHandlers;
import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.HashMap;
import discordbot.BotSettings;
import discordbot.Main;
import discordbot.common.Enums.LogLevel;
import discordbot.common.Enums.Settings;

public class AuctionsRoute {
    public boolean success;
    public int page;
    public int totalPages;
    public int totalAuctions;
    public double lastUpdated;
    public AuctionsRouteProduct[] auctions;

    public static ArrayList<AuctionsRouteProduct> GetRoute() {
        return GetRoute(null, 100);
    }

    public static ArrayList<AuctionsRouteProduct> GetRoute(HashMap<String, Double> CachedItems, int pages) {
        ArrayList<AuctionsRouteProduct> auctions = new ArrayList<AuctionsRouteProduct>();
        int i = 0;

        try {

            for (i = 0; i < pages; i++) {
                // Main.Utility.LogPerformance("Total Start", false);

                String json = new String("");
                int maxRetry = 3;
                int currentRetry = 0;

                do {
                    try {
                        URI uri = new URI(MessageFormat.format("{0}/skyblock/auctions?page={1}", BotSettings.Get(Settings.HYPIXEL_API_BASE_URL), i));
                        HttpRequest request = HttpRequest.newBuilder(uri).header("API-Key", BotSettings.Get(Settings.HYPIXEL_BOT_KEY)).build();
                        json = Main.Client.send(request, BodyHandlers.ofString()).body();
                        Main.Utility.Log(LogLevel.NONE, json);
                        AuctionsRouteProduct product = Main.Gson.fromJson(json, AuctionsRouteProduct.class);
                        // System.out.println(product.);
                        currentRetry = maxRetry + 1; // exit loop
                    } catch (Exception e) {
                        Thread.sleep(100);
                        currentRetry += 1;
                        Main.Utility.Log(LogLevel.WARN, MessageFormat.format("Fetch failed! ({0})", e));

                        if (currentRetry >= maxRetry)
                            throw new Exception(MessageFormat.format("Reached max retry limit! Throwing... {0}", e));
                    }
                } while (currentRetry <= maxRetry);

            }

        } catch (Exception err) {

        }

        return auctions;
    }
}

//                 async Task setJson() => json = await Program.Client.GetStringAsync($"{Settings.HYPIXEL_API_BASE_URL}/skyblock/auctions?page={i}");

//                 try
//                 {
//                     await setJson();
//                 }

//                 catch (Exception)
//                 {
//                     currentRetry++;
//                     Thread.Sleep(100);

//                     Program.Utility.Log(Enums.LogLevel.WARN, $"Fetch failed! ({currentRetry})");
//                     if (currentRetry >= maxRetry)
//                         throw;

//                     await setJson();
//                 }

//                 JsonNode? result = JsonArray.Parse(json);

//                 if (result == null)
//                 {
//                     Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should not be null.");
//                     return null;
//                 }

//                 if (result["success"] == null)
//                 {
//                     Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null when the result should always contains a 'success' field.");
//                     return null;
//                 }

//                 if (result["success"]!.Equals(false))
//                 {
//                     Program.Utility.Log(Enums.LogLevel.WARN, "Success = false, returning...");
//                     return null;
//                 }

//                 AuctionsRoute? auctionsRoute = JsonConvert.DeserializeObject<AuctionsRoute>(json);

//                 if (auctionsRoute == null)
//                 {
//                     Program.Utility.Log(Enums.LogLevel.ERROR, "Unexpected null while converting response to C# class.");
//                     return null;
//                 }

//                 foreach (AuctionsRouteProduct product in auctionsRoute.auctions)
//                 {
//                     if (CachedItems != null)
//                         if (!CachedItems.ContainsKey(product.uuid))
//                             CachedItems.Add(product.uuid, default);
//                         else
//                             continue;

//                     auctions.Add(product);
//                 }

//                 Program.Utility.Log(Enums.LogLevel.NONE, $"Page {i + 1} of auctions fetched.", true, false);

//                 if (auctionsRoute.auctions.Length < 1000)
//                 {
//                     Program.Utility.Log(Enums.LogLevel.NONE, $"Found end of length at page {i + 1}. Closing...");
//                     break;
//                 }

//                 await Task.Delay(10); // make sure we dont request too fast
//             }

//             return auctions;
//         }

//         catch (Exception e)
//         {
//             Program.Utility.Log(Enums.LogLevel.ERROR, $"Unexpected error on page {i}: {e}");
//             return null;
//         }
//     }
// }