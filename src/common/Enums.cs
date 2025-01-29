using Discord.Interactions;

namespace Enums
{
    public enum LogLevel
    {
        NONE,
        WARN,
        ERROR,
    }

    public class BazaarItems
    {
        /// <summary>
        /// Enum version of the items, that also uses the Discord .NET attributes for commands
        /// </summary>
        public enum Enum
        {
            [ChoiceDisplay("Ink Sac")]
            INK_SAC_3,
            [ChoiceDisplay("Enchanted Ink Sac")]
            INK_SAC_4,
        }

        /// <summary>
        /// Translates the Hypixel API item names to understandable names (wtf is INK_SAC_4)
        /// </summary>
        public static readonly Dictionary<string, string> Dictionary = new()
        {
            { "INK_SAC_3", "Ink Sac" },
            { "INK_SAC_4", "Enchanted Ink Sac" },
        };
    };
}