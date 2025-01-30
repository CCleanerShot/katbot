/// <summary>
/// Random class for helper functions
/// </summary>
public static class Utility
{
    static void GetBazaar()
    {

    }

    /// <summary>
    /// Picks a random number from the given range.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int NextRange(int min, int max)
    {
        float value = new Random().NextSingle();
        int difference = max - min;
        return (int)Math.Round((value * difference) + min);
    }

    /// <summary>
    /// Current way of logging. Will do some other stuff later.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public static void Log(Enums.LogLevel logLevel, string message)
    {
        string prefix = "";
        switch (logLevel)
        {
            case Enums.LogLevel.NONE:
                prefix = ">";
                break;
            case Enums.LogLevel.WARN:
                prefix = "?";
                break;
            case Enums.LogLevel.ERROR:
                prefix = "!";
                break;
            default:
                throw new NotImplementedException("Implement this");
        }

        Console.WriteLine($"{prefix} {message}");
    }
}