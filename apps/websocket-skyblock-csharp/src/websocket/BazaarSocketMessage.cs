public class BazaarSocketMessage
{
    public BazaarRouteProduct LiveSummary;
    public BazaarItem RequestedItem;

    public BazaarSocketMessage(BazaarRouteProduct _LiveSummary, BazaarItem _RequestedItem)
    {
        LiveSummary = _LiveSummary;
        RequestedItem = _RequestedItem;
    }
}