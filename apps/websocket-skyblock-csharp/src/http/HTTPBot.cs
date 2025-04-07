using System.Net;
using System.Net.Sockets;
using System.Text;

public static class HTTPBot
{
    static HttpListener listener = default!;
    static readonly string response = "Hello";
    static bool running = true;

    public static async Task Load()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://127.0.0.1:{Settings.HTTP_SERVER_PORT}/");
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