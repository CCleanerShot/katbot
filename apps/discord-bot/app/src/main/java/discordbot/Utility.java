package discordbot;

import java.text.MessageFormat;
import java.time.Instant;
import java.util.Random;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import discordbot.common.Enums;

/**
 * Random class for helper functions
 */
public class Utility {
    public String LogLine = "";

    // TODO: look for alternatives, maybe a listener on the process itself?
    // ~Utility()
    // {
    // Program.Utility.Log(Enums.LogLevel.NONE, "Closing...");
    // using StreamWriter writer = new StreamWriter($"{DateTime.Now}.txt");
    // writer.Write(Program.Utility.LogLine);
    // }

    /**
     * TODO: finish
     */
    public String GetPath(String relativePath) {
        String currentDirectory = System.getProperty("user.dir");
        return currentDirectory + relativePath;
    }

    /**
     * Current way of logging. Will do some other stuff later (lie).
     */
    public void Log(Enums.LogLevel logLevel, String message, boolean timeStamp, boolean addToLogFile) {
        String prefix = "";

        switch (logLevel) {
            case Enums.LogLevel.WARN:
                prefix = "?";
                break;
            case Enums.LogLevel.ERROR:
                prefix = "!";
                break;
            default:
            case Enums.LogLevel.NONE:
                prefix = ">";
                break;
        }

        String fullMessage;

        if (timeStamp)
            fullMessage = MessageFormat.format("{0} [{1}] {2}", prefix, Instant.now(), message);
        else
            fullMessage = MessageFormat.format("{0} {1}", prefix, message);

        if (addToLogFile)
            LogLine += fullMessage + "\n";
        else
            fullMessage = "(TEMP) " + fullMessage;

        System.out.println(fullMessage);
    }

    /**
     * Current way of logging. Will do some other stuff later (lie).
     */
    public void Log(Enums.LogLevel logLevel, String message) {
        Log(logLevel, message, false, true);
    }

    /**
     * Logs the current RAM usage into the console.
     */
    public void LogPerformance(String additionalMessage, boolean addToLogFile) {
        long memory = Runtime.getRuntime().totalMemory() / 1000000; // MB
        Log(Enums.LogLevel.NONE, MessageFormat.format("{0}MB ({1})", memory, additionalMessage), false, addToLogFile);
    }

    /**
     * Picks a random number from the given range (inclusive).
     */
    public int NextRange(int min, int max) {
        float value = new Random().nextInt();
        int difference = max - min;
        return Math.round((value * difference) + min);
    }

    /**
     * Returns a number as a string, spaced out by given units.
     */
    public String SpaceNumber(int input, int spacing) {
        Pattern pattern = Pattern.compile(MessageFormat.format("\\d{{0}}", spacing));
        Matcher matcher = pattern.matcher(Integer.toString(input));
        return matcher.group();
    }

    /**
     * Spaces out the string so that it is left-leaning on the max spacing.
     * EXAMPLE: "test" on a maxSpacing of 10 becomes " test".
     */
    public String SpaceString(String input, int maxSpacing) {
        return " ".repeat(Math.max(maxSpacing - input.length(), 0)) + input;
    }

}
