using Enums;

public class SocketMessage
{
    public int id;
    public SocketMessageType type;
    public List<AuctionSocketMessage> auctionSocketMessages = new List<AuctionSocketMessage>();
    public List<BazaarItem> bazaarBuys = new List<BazaarItem>();
    public List<BazaarItem> bazaarSells = new List<BazaarItem>();

    public SocketMessage()
    {
        id = new Random().Next();
    }
}