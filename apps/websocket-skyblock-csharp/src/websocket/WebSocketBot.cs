using System.Net.Sockets;
using Fleck;
using MongoDB.Driver;
using oslo.crypto.sha2;

public static partial class WebSocketBot
{
    // NOTE: idk how to manually upgrade connections in C# with these packages LOL
    static Dictionary<Session, IWebSocketConnection> Connections = new Dictionary<Session, IWebSocketConnection>();
    static WebSocketServer Server = default!;

    public static void Load()
    {
        string url = "ws://0.0.0.0:3000";
        Server = new WebSocketServer(url);
        Server.Start(async (ws) =>
        {
            IDictionary<string, string> Headers = ws.ConnectionInfo.Headers;

            if (!Headers.ContainsKey("cookie"))
            {
                ws.Close();
                return;
            }

            string cookies = ws.ConnectionInfo.Headers["Cookie"];
            Dictionary<string, string> kvPairs = Utility.CookieStringAsDict(cookies);

            if (!kvPairs.ContainsKey("session"))
            {
                ws.Close();
                return;
            }

            string sessionId = kvPairs["session"];
            Session? session = await ValidateSessionToken(sessionId);

            if (session == null)
            {
                ws.Close();
                return;
            }

            AddEvents(session, ws);
            Connections.Add(session, ws);
            Utility.Log(Enums.LogLevel.NONE, "Added ws connection.");
        });
    }

    public static void SendAuctionData()
    {
        foreach (var (session, connection) in Connections)
        {
            SocketMessage message = new SocketMessage();

            foreach (var (k, v) in MongoBot.ElgibleAuctionBuys)
                if (k == session.UserId)
                    foreach (var products in v)
                        message.auctionItemsWithBuys.Add(products);

            connection.Send(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }
    }

    public static void SendBazaarData()
    {
        foreach (var (session, connection) in Connections)
        {
            SocketMessage message = new SocketMessage();

            foreach (var (k, v) in MongoBot.ElgibleBazaarBuys)
                if (k == session.UserId)
                    foreach (BazaarItem item in v)
                        message.bazaarBuys.Add(item);

            foreach (var (k, v) in MongoBot.ElgibleBazaarSells)
                if (k == session.UserId)
                    foreach (BazaarItem item in v)
                        message.bazaarSells.Add(item);

            connection.Send(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }
    }


    static void AddEvents(Session session, IWebSocketConnection ws)
    {
        void OnClose() => Connections.Remove(session);
        void OnMessage(string message) => Console.WriteLine(message);
        ws.OnClose += OnClose;
        ws.OnMessage += OnMessage;
    }

    static async Task<Session?> ValidateSessionToken(string token)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(token);
        SHA256 hash = new SHA256();
        hash.Update(data);
        byte[] result = hash.Digest();
        string sessionId = Utility.EncodeHexLowercase(result);
        FilterDefinition<Session> filter = new FilterDefinitionBuilder<Session>().Where(e => e.ID == sessionId);
        List<Session>? sessions = await MongoBot.Session.FindList(filter);

        if (sessions.Count == 0)
            return null;

        Session session = sessions[0];

        if (DateTime.Now >= session.ExpiresAt)
        {
            await MongoBot.Session.DeleteOneAsync(filter);
            return null;
        }

        int days = Int32.Parse(Settings.MONGODB_SESSION_DAY_LENGTH);

        if (DateTime.Now >= session.ExpiresAt.AddDays(days / 2))
        {
            session.ExpiresAt = DateTime.Now.AddDays(days);
            await MongoBot.Session.UpdateOneAsync(filter, new UpdateDefinitionBuilder<Session>().Set(e => e.ExpiresAt, session.ExpiresAt));
        }

        return session;
    }
}