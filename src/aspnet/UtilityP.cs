namespace skyblock_bot.Pages;

public enum RouteP
{
    DISCORD,
    HOME,
    HTMX_AUCTION_BUYS,
    HTMX_AUCTION_ITEMS,
    HTMX_AUCTION_TAGS,
    HTMX_BAZAAR_BUY,
    HTMX_BAZAAR_ITEM,
    HTMX_BAZAAR_SELL,
    LOGIN,
    MODAL_BAZAAR_BUY_ADD,
    MODAL_BAZAAR_BUY_EDIT,
    MODAL_BAZAAR_SELL_ADD,
    MODAL_BAZAAR_SELL_EDIT,
    MODAL_CLEAR,
    SKYBLOCK,
    SKYBLOCK_AUCTIONS,
    SKYBLOCK_BAZAAR,
}

public enum Target
{
    BZ_BUY_RESULTS,
    BZ_SELL_RESULTS,
    LOADING_ICON,
    MODAL,
    MODAL_CONTENT,
}



public static class UtilityP
{
    public static Dictionary<Target, string> HTMX = new Dictionary<Target, string>()
    {
        {Target.BZ_BUY_RESULTS, "bazaar-buy-results" },
        {Target.BZ_SELL_RESULTS, "bazaar-sell-results" },
        {Target.LOADING_ICON, "loading-icon" },
        {Target.MODAL, "root-modal" },
        {Target.MODAL_CONTENT, "root-modal-content" },
    };

    public static Dictionary<RouteP, string> Routes = new Dictionary<RouteP, string>()
    {
        { RouteP.DISCORD, "/discord"},
        { RouteP.HOME, "/"},
        { RouteP.HTMX_AUCTION_BUYS, "/htmx/auction-buy"},
        { RouteP.HTMX_AUCTION_ITEMS, "/htmx/auction-item"},
        { RouteP.HTMX_AUCTION_TAGS, "/htmx/auction-tag"},
        { RouteP.HTMX_BAZAAR_BUY, "/htmx/bazaar-buy"},
        { RouteP.HTMX_BAZAAR_ITEM, "/htmx/bazaar-item"},
        { RouteP.HTMX_BAZAAR_SELL, "/htmx/bazaar-sell"},
        { RouteP.LOGIN, "/login"},
        { RouteP.MODAL_BAZAAR_BUY_ADD, "/modals/bazaar-buy-add"},
        { RouteP.MODAL_BAZAAR_BUY_EDIT, "/modals/bazaar-buy-edit"},
        { RouteP.MODAL_BAZAAR_SELL_ADD, "/modals/bazaar-sell-add"},
        { RouteP.MODAL_BAZAAR_SELL_EDIT, "/modals/bazaar-sell-edit"},
        { RouteP.MODAL_CLEAR, "/modals/clear"},
        { RouteP.SKYBLOCK, "/skyblock"},
        { RouteP.SKYBLOCK_AUCTIONS, "/skyblock/auctions"},
        { RouteP.SKYBLOCK_BAZAAR, "/skyblock/bazaar"},
    };
}
