public class BazaarRouteProduct
{
    public string product_id;
    /// <summary>
    /// The list of the top buy orders.
    /// </summary>
    public BazaarRouteSummaryItem[] buy_summary;
    /// <summary>
    /// The list of the top sell orders.
    /// </summary>
    public BazaarRouteSummaryItem[] sell_summary;
    public BazaarRouteQuickStatus quick_status;

    public BazaarRouteProduct(string _product_id, BazaarRouteSummaryItem[] _sell_summary, BazaarRouteSummaryItem[] _buy_summary, BazaarRouteQuickStatus _quick_status)
    {
        product_id = _product_id;
        sell_summary = _sell_summary;
        buy_summary = _buy_summary;
        quick_status = _quick_status;
    }
}