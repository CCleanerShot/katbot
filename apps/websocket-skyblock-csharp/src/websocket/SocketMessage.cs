public class SocketMessage
{
    public int ID;
    public List<AuctionItemsWithBuy> auctionItemsWithBuys = new List<AuctionItemsWithBuy>();
    public List<BazaarItem> bazaarBuys = new List<BazaarItem>();
    public List<BazaarItem> bazaarSells = new List<BazaarItem>();

    public SocketMessage()
    {
        ID = new Random().Next();
    }
}