using Enums;

public class SocketMessage
{
    public int id;
    public SocketMessageType type;
    public List<AuctionSocketMessage> auctionSocketMessages = new List<AuctionSocketMessage>();
    public List<BazaarSocketMessage> bazaarSocketMessagesBuy = new List<BazaarSocketMessage>();
    public List<BazaarSocketMessage> bazaarSocketMessagesSell = new List<BazaarSocketMessage>();

    public SocketMessage()
    {
        id = new Random().Next();
    }
}