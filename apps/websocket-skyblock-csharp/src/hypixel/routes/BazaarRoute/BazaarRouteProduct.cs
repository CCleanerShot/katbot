public class BazaarRouteProduct
{
    public string product_id = default!;
    /// <summary>
    /// The list of the top buy orders.
    /// </summary>
    public BazaarRouteSummaryItem[] buy_summary = default!;
    /// <summary>
    /// The list of the top sell orders.
    /// </summary>
    public BazaarRouteSummaryItem[] sell_summary = default!;
    public BazaarRouteQuickStatus quick_status = default!;
}