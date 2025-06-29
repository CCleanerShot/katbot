package discordbot;

import java.util.HashMap;
import com.google.gson.Gson;
import java.net.http.HttpClient;
import discordbot.hypixel.routes.AuctionsRoute.AuctionsRoute;

// TODO: figure out how to handle executing java code when the program exits
// TODO: figure out if i need @DataMember for mongoDB schemas and getter settter for _id

public class Main {
    public static HttpClient Client;
    public static Gson Gson;
    public static Utility Utility;

    public static void main(String[] args) {
        Client = HttpClient.newHttpClient();
        Gson = new Gson();
        Utility = new Utility();

        BotSettings.Load();
        AuctionsRoute.GetRoute(new HashMap<String, Double>(), 2);
    }
}
