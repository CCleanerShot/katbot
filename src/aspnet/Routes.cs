namespace skyblock_bot.Pages;

public enum RouteP
{
    DISCORD,
    HOME,
    LOGIN,
    SKYBLOCK,
    SKYBLOCK_AUCTIONS,
    SKYBLOCK_BAZAAR,
}


public static class UtilityP
{
    public static Dictionary<RouteP, string> Routes = new Dictionary<RouteP, string>()
    {
        { RouteP.DISCORD, "/discord"},
        { RouteP.HOME, "/"},
        { RouteP.LOGIN, "/login"},
        { RouteP.SKYBLOCK, "/skyblock"},
        { RouteP.SKYBLOCK_AUCTIONS, "/skyblock/auctions"},
        { RouteP.SKYBLOCK_BAZAAR, "/skyblock/bazaar"},
    };
}
