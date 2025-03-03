public class BazaarTableModel : AuthorizedModel
{
    public List<BazaarItem> BazaarItems = new List<BazaarItem>();
    public BazaarTableModel(ILogger<BazaarTableModel> logger) : base(logger) { }
}