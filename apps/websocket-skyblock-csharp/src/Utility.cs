using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

/// <summary>
/// Random class for helper functions
/// </summary>
public static class Utility
{
    public static string LogLine = "";

    /// <summary>
    /// Returns the passed in cookies (from a string) into a Dictionary with matching key + pair values.
    /// </summary>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static Dictionary<string, string> CookieStringAsDict(string cookies)
    {
        Regex regex = new Regex("[\\s=;]+");
        string[] match = regex.Split(cookies);
        Dictionary<string, string> kvPairs = new Dictionary<string, string>();

        for (int i = 0; i < match.Length; i++)
        {
            string item = match[i];

            if (i % 2 == 0)
                kvPairs.Add(item, "");
            else
                kvPairs[match[i - 1]] = item;
        }

        return kvPairs;
    }

    /// <summary>
    /// Original code from https://github.com/oslo-project/encoding/blob/main/src/hex.ts, re-purposed to use here.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string EncodeHexLowercase(byte[] data)
    {
        string alphabetLowercase = "0123456789abcdef";
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += alphabetLowercase[data[i] >> 4];
            result += alphabetLowercase[data[i] & 0x0f];
        }

        return result;
    }


    /// <summary>
    /// <para> Fills a passed array with a given value, from `start` index to `end` index. </para>
    /// <para> This operation is a mutation of the original array. </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public static void Fill<T>(T[] array, T value, int start, int end)
    {
        if (array.Length == 1)
        {
            array[0] = value;
            return;
        }

        T[] newItems = new T[end - start].Select(e => value).ToArray();
        newItems.CopyTo(array, start);
    }

    /// <summary>
    /// Returns either the relative or absolute path, depending on the environment.
    /// Useful for when some functions require absolute paths on Windows, and relative on Linux. 
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public static string GetASPPath(string relativePath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return relativePath;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Environment.CurrentDirectory + relativePath;
        else
            throw new NotImplementedException("Detected application launch in something not Windows/Linux, throwing!");
    }


    /// <summary>
    /// Current way of logging. Will do some other stuff later.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public static void Log(Enums.LogLevel logLevel, string message, bool timeStamp = false, bool addToLogFile = true)
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
    /// Logs the current RAM usage into the console.
    /// </summary>
    /// <param name="additionalMessage"></param>
    public static void LogPerformance(string additionalMessage = "", bool addToLogFile = false)
    {
        long memory = GC.GetTotalMemory(true);
        Log(Enums.LogLevel.NONE, $"{memory / 1000000}MB ({additionalMessage})", false, addToLogFile);
    }

    /// <summary>
    /// Picks a random number from the given range (inclusive).
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

    /// <summary>
    /// Strips the string of extraneous characters such as "
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string StripSpecial(string input)
    {
        Regex regex = new Regex("§.");
        return regex.Replace(input, "");
    }
}