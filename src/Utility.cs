using System.Text.RegularExpressions;

/// <summary>
/// Random class for helper functions
/// </summary>
public static class Utility
{
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
    /// Returns a number as a string, spaced out by given units.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="spacing"></param>
    /// <returns></returns>
    public static string SpaceNumber(int input, int spacing)
    {
        return string.Join(" ", Regex.Split(input.ToString(), $@"\d{{{spacing}}}"));
    }

    /// <summary>
    /// Spaces out the string so that it is left-leaning on the max spacing
    /// EXAMPLE: "test" on a maxSpacing of 10 becomes "      test".
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxSpacing"></param>
    /// <returns></returns>
    public static string SpaceString(string input, int maxSpacing)
    {
        return new string(' ', (int)MathF.Max(maxSpacing - input.Length, 0)) + input;
    }

    /// <summary>
    /// Short for SpaceString.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxSpacing"></param>
    /// <returns></returns>
    public static string SS(string input, int maxSpacing)
    {
        return SpaceString(input, maxSpacing);
    }
}