public class BazaarRouteSummaryItem
{
    public double amount;
    public float pricePerUnit;
    public int orders;

    public BazaarRouteSummaryItem(double _amount, float _pricePerUnit, int _orders)
    {
        amount = _amount;
        pricePerUnit = _pricePerUnit;
        orders = _orders;
    }
}
