public readonly struct AuctionItemsWithBuy
{
    public readonly List<AuctionsRouteProductMinimal> LiveItems;
    public readonly AuctionBuy BuyItem;

    public AuctionItemsWithBuy(List<AuctionsRouteProductMinimal> _LiveItems, AuctionBuy _BuyItem)
    {
        LiveItems = _LiveItems;
        BuyItem = _BuyItem;
    }
}