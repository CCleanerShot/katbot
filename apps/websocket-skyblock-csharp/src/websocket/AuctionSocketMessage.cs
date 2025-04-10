public class AuctionSocketMessage
{
    public List<AuctionsRouteProductMinimal> LiveItems;
    public AuctionBuy RequestedItem;

    public AuctionSocketMessage(List<AuctionsRouteProductMinimal> _LiveItems, AuctionBuy _RequestedItem)
    {
        LiveItems = _LiveItems;
        RequestedItem = _RequestedItem;
    }
}