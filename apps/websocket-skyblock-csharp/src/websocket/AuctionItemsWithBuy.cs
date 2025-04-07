public class AuctionItemsWithBuy
{
    public List<AuctionsRouteProductMinimal> LiveItems;
    public AuctionBuy BuyItem;

    public AuctionItemsWithBuy(List<AuctionsRouteProductMinimal> _LiveItems, AuctionBuy _BuyItem)
    {
        LiveItems = _LiveItems;
        BuyItem = _BuyItem;
    }
}