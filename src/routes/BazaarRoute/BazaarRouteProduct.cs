public partial class BazaarRouteProduct
{
    public string product_id;
    public BazaarRouteSellSummaryItem[] sell_summary;

    public BazaarRouteProduct(string _product_id, BazaarRouteSellSummaryItem[] _sell_summary)
    {
        product_id = _product_id;
        sell_summary = _sell_summary;
    }
}