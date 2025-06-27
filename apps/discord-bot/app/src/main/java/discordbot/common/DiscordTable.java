package discordbot.common;

import java.util.Set;
import java.util.HashMap;
import java.util.ArrayList;
import java.util.Map.Entry;
import java.text.MessageFormat;

import discordbot.Main;

/**
 * Helper table class that can generate a text based on the columns.
 */
public class DiscordTable<T extends Enum<?>> {
    public String Header = "";
    public HashMap<T, ArrayList<String>> Table = new HashMap<T, ArrayList<String>>();

    public DiscordTable(String _Header, Class<T> _Enum) {
        Header = _Header;

        for (T value : _Enum.getEnumConstants())
            Table.put(value, new ArrayList<String>());
    }

    /**
     * Combines the Construct() of this table with additional tables inside 1 codeblock.
     */
    @SafeVarargs
    public static <T extends Enum<?>> String ConstructCombine(DiscordTable<T>... otherTables) {
        String result = "```\n";

        ArrayList<DiscordTable<T>> _otherTables = new ArrayList<DiscordTable<T>>();

        for (int i = 0; i < otherTables.length; i++)
            _otherTables.add(otherTables[i]);

        for (int i = 0; i < otherTables.length; i++)
            result += otherTables[i].Construct(_otherTables);

        result += "```";
        return result;
    }

    /**
     * Converts the Table field as a literal string table in a codeblock.
     */
    public String Construct() {
        return Construct(new ArrayList<DiscordTable<T>>());
    }

    /**
     * Converts the Table field as a literal string table in a codeblock.
     */
    public String Construct(ArrayList<DiscordTable<T>> otherTables) {
        HashMap<T, Integer> MaxValues = new HashMap<T, Integer>();

        if (otherTables.size() == 0)
            otherTables.add(this);

        for (DiscordTable<T> discordTable : otherTables) {
            for (Entry<T, ArrayList<String>> entry : discordTable.Table.entrySet()) {
                T k = entry.getKey();
                ArrayList<String> v = entry.getValue();

                if (MaxValues.containsKey(k))
                    MaxValues.put(k, 0);

                int maxValue = Math.max(MaxValues.get(k), k.toString().length());

                for (String word : v)
                    if (maxValue < word.length())
                        maxValue = word.length();

                MaxValues.put(k, maxValue);
            }
        }

        String result = "";

        if (otherTables.get(0) == this)
            result += "```\n";

        result += MessageFormat.format("**{0}**\n", Header);

        // TODO: refactor. maybe not.
        Set<Entry<T, ArrayList<String>>> _entries = Table.entrySet();
        ArrayList<Entry<T, ArrayList<String>>> entries = new ArrayList<Entry<T, ArrayList<String>>>();

        for (Entry<T, ArrayList<String>> key : _entries)
            entries.add(key);

        // headers
        for (int i = 0; i < Table.size(); i++) {
            Entry<T, ArrayList<String>> entry = entries.get(i);
            T k = entry.getKey();

            result += Main.Utility.SpaceString(k.toString(), MaxValues.get(k));

            if (i != Table.size() - 1)
                result += "|";
            else
                result += "\n";
        }

        // values
        for (int row = 0; row < entries.get(0).getValue().size(); row++) {
            for (int col = 0; col < Table.size(); col++) {
                Entry<T, ArrayList<String>> entry = entries.get(col);
                T k = entry.getKey();
                ArrayList<String> v = entry.getValue();
                String word = entry.getValue().get(row);

                result += Main.Utility.SpaceString(word, MaxValues.get(k));

                if (col != Table.size() - 1)
                    result += "|";
                else if (row != v.size() - 1)
                    result += "\n";
            }
        }

        if (otherTables.get(0) == this)
            result += "```\n";

        return result;
    }
}

//         if (otherTables == null)
//             result += "```\n";

//         return result;
//     }
// }