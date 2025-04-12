using System.Net;
using System.Text;

public static class HTTPBot
{
    static HttpListener listener = default!;
    static readonly string response = "Hello";
    static bool running = true;

    public static async Task Load()
    {
        string PROTOCOL;

        if (Settings.ENVIRONMENT == "development")
            PROTOCOL = "http";
        else if (Settings.ENVIRONMENT == "production")
            PROTOCOL = "https";
        else
            throw new NotImplementedException($"Unknown environment setting at {Settings.ENVIRONMENT}");

        listener = new HttpListener();
        listener.Prefixes.Add($"{PROTOCOL}://0.0.0.0:{Settings.PORT_WEBSOCKET}/");
        listener.Start();

        Utility.Log(Enums.LogLevel.NONE, "HTTP Listener active.");

        while (running)
        {
            HttpListenerContext context = listener.GetContext();
            Utility.Log(Enums.LogLevel.NONE, $"Incoming request... {context.Request.AsString()}");
            context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(response);
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            using (Stream stream = context.Response.OutputStream)
            using (StreamWriter streamWriter = new StreamWriter(stream))
                streamWriter.Write(response);

            context.Response.Close();

            MongoBot.AuctionBuysRecentlyUpdated = true;
            Utility.Log(Enums.LogLevel.NONE, "Request closed.");
        }
    }
}