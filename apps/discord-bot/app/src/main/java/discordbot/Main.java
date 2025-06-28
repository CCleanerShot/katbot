package discordbot;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpRequest.BodyPublishers;
import java.net.http.HttpResponse.BodyHandler;
import java.net.http.HttpResponse.BodyHandlers;
import java.net.http.HttpResponse;
import java.nio.file.Paths;
import java.text.MessageFormat;
import java.time.Duration;

import discordbot.common.Enums.LogLevel;

// TODO: figure out how to handle executing java code when the program exits
// TODO: figure out if i need @DataMember for mongoDB schemas and getter settter for _id

public class Main {
    // public static HttpClient Client = default!;
    public static Utility Utility = new Utility();

    public static void main(String[] args) {
        HttpClient client = HttpClient.newBuilder()
                .build();

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create("https://foo.com/"))
                .timeout(Duration.ofMinutes(2))
                .header("Content-Type", "application/json")
                .build();

        try {
            HttpResponse<String> response = client.send(request, BodyHandlers.ofString());
        } catch (IOException e) {
            Main.Utility.Log(LogLevel.ERROR, MessageFormat.format("", e));
        } catch (InterruptedException e) {
            Main.Utility.Log(LogLevel.ERROR, MessageFormat.format("aa", e));
        }
    }
}
