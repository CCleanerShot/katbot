using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Driver.Core.Connections;

/// <summary>
/// Helper table class that can generate a text based on the columns.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DiscordTable<T> where T : Enum
{
    public string Header = "";
    public Dictionary<T, List<string>> Table = new Dictionary<T, List<string>>();

    public DiscordTable(string _Header)
    {
        Header = _Header;

        foreach (T value in Enum.GetValues(typeof(T)))
            Table.Add(value, new List<string>());
    }

    /// <summary>
    /// Combines the Construct() of this table with additional tables inside 1 codeblock.
    /// TODO: implement
    /// </summary>
    public static string ConstructCombine(params DiscordTable<T>[] otherTables)
    {
        string result = "```\n";

        for (int i = 0; i < otherTables.Length; i++)
            result += otherTables.ElementAt(i).Construct(otherTables);

        result += "```";
        return result;
    }

    /// <summary>
    /// Converts the Table field as a literal string table in a codeblock.
    /// </summary>
    /// <param name="otherTables">Whether or not this is constructed with other tables in mind</param>
    /// <returns></returns>
    public string Construct(DiscordTable<T>[]? otherTables = null)
    {
        Dictionary<T, int> MaxValues = new Dictionary<T, int>();

        foreach (var discordTable in otherTables ?? [this])
        {
            foreach (var (k, v) in discordTable.Table)
            {
                if (!MaxValues.ContainsKey(k))
                    MaxValues.Add(k, 0);

                int maxValue = (int)MathF.Max(MaxValues[k], k.ToString().Length);

                foreach (string word in v)
                {
                    if (maxValue < word.Length)
                        maxValue = word.Length;
                }

                MaxValues[k] = maxValue;
            }
        }

        string result = "";

        if (otherTables == null)
            result += "```\n";

        result += $"**{Header}**\n";

        // headers
        for (int i = 0; i < Table.Count; i++)
        {
            var (k, v) = Table.ElementAt(i);
            result += $"{Utility.SpaceString(k.ToString(), MaxValues[k])}";

            if (i != Table.Count - 1)
                result += "|";
            else
                result += "\n";
        }

        // values
        for (int row = 0; row < Table.ElementAt(0).Value.Count; row++)
        {

            for (int col = 0; col < Table.Count; col++)
            {
                var (k, v) = Table.ElementAt(col);
                string word = Table.ElementAt(col).Value[row];

                result += $"{Utility.SpaceString(word, MaxValues[k])}";

                if (col != Table.Count - 1)
                    result += "|";
                else if (row != Table.ElementAt(0).Value.Count - 1)
                    result += "\n";
            }
        }

        if (otherTables == null)
            result += "```\n";

        return result;
    }
}
