using System.Text.RegularExpressions;
using Cyotek.Data.Nbt;

public partial class AuctionsRouteProductNBT
{
    /// <summary>
    /// Cleans the NBT data to something human-readable and more optimized.
    /// </summary>
    void Clean()
    {
        NAME.Value = Program.Utility.StripSpecial(NAME.Value);

        BASE.Value.Remove(BASE.Value["id"]);
        BASE.Value.Remove(BASE.Value["Count"]);
        // BASE.Value.Remove(BASE.Value["tag"]);
        BASE.Value.Remove(BASE.Value["Damage"]);

        // TAG.Value.Remove(TAG.Value["ExtraAttributes"]);
        // TAG.Value.Remove(TAG.Value["display"]);
        TAG.Value.Remove(TAG.Value["HideFlags"]);

        if (TAG.Value.Contains("SkullOwner"))
            TAG.Value.Remove(TAG.Value["SkullOwner"]);

        DISPLAY.Value.Remove(DISPLAY.Value["Lore"]);

        if (TAG.Value.Contains("color"))
            TAG.Value.Remove(TAG.Value["color"]);
    }

    /// <summary>
    /// Trims the tag of excessive data (the instance doesn't have alot of RAM to spare).
    /// </summary>
    void CleanTag(Cyotek.Data.Nbt.Tag tag)
    {

    }
}