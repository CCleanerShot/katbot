/// <summary>
/// Random class for helper functions
/// </summary>
public partial class Utility
{
    static void GetBazaar()
    {

    }

    public static void Log(Enums.LogLevel logLevel, string message)
    {
        string prefix = "";
        switch (logLevel)
        {
            case Enums.LogLevel.NONE:
                prefix = "> ";
                break;
            case Enums.LogLevel.ERROR:
                prefix = "? ";
                break;
            case Enums.LogLevel.WARN:
                prefix = "! ";
                break;
            default:
                throw new NotImplementedException("Implement this");
        }

        Console.WriteLine($"{prefix} {message}");
    }
}