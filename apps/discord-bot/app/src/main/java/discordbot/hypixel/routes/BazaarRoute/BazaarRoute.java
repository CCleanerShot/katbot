package discordbot.hypixel.routes.BazaarRoute;

import java.net.URI;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse.BodyHandlers;
import java.text.MessageFormat;
import java.util.HashMap;

import discordbot.BotSettings;
import discordbot.Main;
import discordbot.common.Enums.LogLevel;
import discordbot.common.Enums.Settings;

public class BazaarRoute {
    public boolean success;
    public double lastUpdated;
    public HashMap<String, BazaarRouteProduct> products = new HashMap<String, BazaarRouteProduct>();

    public static HashMap<String, BazaarRouteProduct> GetRoute() {
        String json = new String();
        int maxRetry = 3;
        int currentRetry = 0;
        HashMap<String, BazaarRouteProduct> products = new HashMap<String, BazaarRouteProduct>();

        try {
            do {
                try {
                    URI uri = new URI(MessageFormat.format("{0}/skyblock/bazaar", BotSettings.Get(Settings.HYPIXEL_API_BASE_URL)));
                    HttpRequest request = HttpRequest.newBuilder(uri).header("API-Key", BotSettings.Get(Settings.HYPIXEL_BOT_KEY)).build();
                    json = Main.Client.send(request, BodyHandlers.ofString()).body();
                    BazaarRoute product = Main.Gson.fromJson(json, BazaarRoute.class);
                    products = product.products;
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

        return products;
    }
}