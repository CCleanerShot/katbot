public class BazaarRouteQuickStatus
{
    public string productId;
    public int buyOrders;
    public float buyPrice;
    public double buyMovingWeek;
    public double buyVolume;
    public int sellOrders;
    public float sellPrice;
    public double sellMovingWeek;
    public double sellVolume;

    public BazaarRouteQuickStatus(string _productId, float _sellPrice, double _sellVolume, double _sellMovingWeek, int _sellOrders, float _buyPrice, double _buyVolume, double _buyMovingWeek, int _buyOrders)
    {
        productId = _productId;
        sellPrice = _sellPrice;
        sellVolume = _sellVolume;
        sellMovingWeek = _sellMovingWeek;
        sellOrders = _sellOrders;
        buyPrice = _buyPrice;
        buyVolume = _buyVolume;
        buyMovingWeek = _buyMovingWeek;
        buyOrders = _buyOrders;
    }
}