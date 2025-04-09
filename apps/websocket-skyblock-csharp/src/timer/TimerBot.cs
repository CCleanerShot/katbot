public static partial class TimerBot
{
    /// <summary>
    /// The timer for the next time Hypixel API is checked for auction items.
    /// </summary>
    public static System.Timers.Timer AuctionTimer = new System.Timers.Timer();
    /// <summary>
    /// The timer for the next time Hypixel API is checked for bazaar items.
    /// </summary>
    public static System.Timers.Timer BazaarTimer = new System.Timers.Timer();

    public static void Load()
    {
        // AuctionTimer.AutoReset = false;
        // AuctionTimer.Interval = 600 * Settings.PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES;
        // AuctionTimer.Elapsed += _AuctionElapsed;
        // AuctionTimer.Start();

        AuctionTimer.AutoReset = true;
        AuctionTimer.Interval = 60000 * Settings.PUBLIC_HYPIXEL_AUCTION_TIMER_MINUTES;
        AuctionTimer.Elapsed += _AuctionElapsed;
        AuctionTimer.Start();

        BazaarTimer.AutoReset = true;
        BazaarTimer.Interval = 60000 * Settings.PUBLIC_HYPIXEL_BAZAAR_TIMER_MINUTES;
        BazaarTimer.Elapsed += _BazaarElapsed;
        BazaarTimer.Start();
    }
}