package discordbot.hypixel.routes.BazaarRoute;

public class BazaarRouteProduct {
    public String product_idd;
    /**
     * The list of the top buy orders.
     */
    public BazaarRouteSummaryItem[] buy_summary;
    /**
     * The list of the top sell orders.
     */
    public BazaarRouteSummaryItem[] sell_summary;
    public BazaarRouteQuickStatus quick_status;
}