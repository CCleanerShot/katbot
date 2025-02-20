using System.Text.RegularExpressions;
using Cyotek.Data.Nbt;

/// <summary>
/// Random class for helper functions
/// </summary>
public class Utility
{
    public string LogLine = "";

    ~Utility()
    {
        Program.Utility.Log(Enums.LogLevel.NONE, "Closing...");
        using StreamWriter writer = new StreamWriter($"{DateTime.Now}.txt");
        writer.Write(Program.Utility.LogLine);
    }

    /// <summary>
    /// Current way of logging. Will do some other stuff later.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public void Log(Enums.LogLevel logLevel, string message, bool timeStamp = false, bool addToLogFile = true)
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

        string fullMessage;

        if (timeStamp)
            fullMessage = $"{prefix} [{DateTime.Now}] {message}";
        else
            fullMessage = $"{prefix} {message}";

        if (addToLogFile)
            LogLine += fullMessage + "\n";
        else
            fullMessage = "(TEMP) " + fullMessage;

        Console.WriteLine(fullMessage);
    }

    /// <summary>
    /// Picks a random number from the given range (inclusive).
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int NextRange(int min, int max)
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
    public string SpaceNumber(int input, int spacing)
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
    public string SpaceString(string input, int maxSpacing)
    {
        return new string(' ', (int)MathF.Max(maxSpacing - input.Length, 0)) + input;
    }

    /// <summary>
    /// Short for SpaceString.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maxSpacing"></param>
    /// <returns></returns>
    public string SS(string input, int maxSpacing)
    {
        return SpaceString(input, maxSpacing);
    }

    /// <summary>
    /// Strips the string of extraneous characters such as "
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string StripSpecial(string input)
    {
        Regex regex = new Regex("§.");
        return regex.Replace(input, "");
    }
}