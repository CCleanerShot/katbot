public class AuctionSocketMessage
{
    public List<AuctionsRouteProductMinimal> LiveItems;
    public AuctionBuy BuyItem;

    public AuctionSocketMessage(List<AuctionsRouteProductMinimal> _LiveItems, AuctionBuy _BuyItem)
    {
        LiveItems = _LiveItems;
        BuyItem = _BuyItem;
    }
}