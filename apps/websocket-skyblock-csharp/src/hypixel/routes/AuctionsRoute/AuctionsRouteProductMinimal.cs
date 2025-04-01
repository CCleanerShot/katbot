using Cyotek.Data.Nbt;
using Newtonsoft.Json.Linq;

/// <summary>
/// Usage: for socket response
/// </summary>
public class AuctionsRouteProductMinimal
{
    #region API_PROPERTIES ordered by API response order
    public string uuid = default!;
    public string auctioneer = default!;
    public ulong start = default!;
    public ulong end = default!;
    public ulong starting_bid = default!;
    public ulong highest_bid_amount = default!;
    #endregion

    public string ITEM_ID = default!;
    public string ITEM_NAME = default!;
    public List<AuctionTag> AuctionTags = new List<AuctionTag>();

    public static AuctionsRouteProductMinimal ToMinimal(AuctionsRouteProductMinimal inheritedItem)
    {
        AuctionsRouteProductMinimal newItem = new AuctionsRouteProductMinimal();
        newItem.uuid = inheritedItem.uuid;
        newItem.auctioneer = inheritedItem.auctioneer;
        newItem.starting_bid = inheritedItem.starting_bid;
        newItem.highest_bid_amount = inheritedItem.highest_bid_amount;
        newItem.ITEM_ID = inheritedItem.ITEM_ID;
        newItem.ITEM_NAME = inheritedItem.ITEM_NAME;
        newItem.AuctionTags = inheritedItem.AuctionTags;
        return newItem;
    }
}
