using MongoDB.Driver;

public class SkyblockBazaarModel : SkyblockBaseModel
{
    public List<BazaarItem> BazaarBuy = new List<BazaarItem>();
    public List<BazaarItem> BazaarItems = new List<BazaarItem>();
    public List<BazaarItem> BazaarSell = new List<BazaarItem>();
    public SkyblockBazaarModel(ILogger<SkyblockBazaarModel> logger) : base(logger) { }
}
