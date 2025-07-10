package discordbot.hypixel.routes.ItemsRoute;

import java.net.URI;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse.BodyHandlers;
import java.text.MessageFormat;
import discordbot.BotSettings;
import discordbot.Main;
import discordbot.common.Enums.LogLevel;
import discordbot.common.Enums.Settings;

public class ItemsRoute {
    /**
     * NOTE: This returns an Object because the data is very dynamic and not worth checking.
     */
    public static Object GetRoute() {
        String json = new String();
        int maxRetry = 3;
        int currentRetry = 0;
        Object product = new Object();

        try {
            do {
                try {
                    URI uri = new URI(MessageFormat.format("{0}/skyblock/bazaar", BotSettings.Get(Settings.HYPIXEL_API_BASE_URL)));
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