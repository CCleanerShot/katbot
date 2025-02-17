/// <summary>
/// Helper table class that can generate a text based on the columns.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DiscordTable<T> where T : Enum
{
    Dictionary<T, List<string>> Table = new Dictionary<T, List<string>>();
    string FinalText = "";

    public DiscordTable()
    {
        foreach (T value in Enum.GetValues(typeof(T)))
            Table.Add(value, new List<string>());
    }

    /// <summary>
    /// Converts the Table field as a literal string table in a codeblock.
    /// TODO: implement
    /// </summary>
    /// <returns></returns>
    string Construct()
    {
        return FinalText;
    }

    /// <summary>
    /// Adds a value to a column.
    /// </summary>
    /// <param name="Column"></param>
    /// <param name="value"></param>
    void AddToRow(T Column, string value)
    {
        Table[Column].Add(value);
    }
}
