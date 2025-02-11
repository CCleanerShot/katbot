using SharpNBT;

public class AuctionsRouteProduct
{
    public string uuid = default!;
    public string auctioneer = default!;
    public string profile_id = default!;
    public string[] coop = default!;
    public ulong start = default!;
    public ulong end = default!;
    public string item_name = default!;
    public string item_lore = default!;
    public string extra = default!;
    public string category = default!;
    public string tier = default!;
    public ulong starting_bid = default!;
    // public string item_bytes = default!; 
    public bool claimed = default!;
    public string[] claimed_bidders = default!; // this might unironically be an empty arrayz
    public ulong highest_bid_amount = default!;

    private string _item_bytes = default!;
    public string item_bytes
    {
        get => _item_bytes;
        set
        {
            _item_bytes = value;
        }
    }
}
