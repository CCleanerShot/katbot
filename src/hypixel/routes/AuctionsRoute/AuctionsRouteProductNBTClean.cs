using System.Text.RegularExpressions;
using Cyotek.Data.Nbt;

public partial class AuctionsRouteProductNBT
{
    /// <summary>
    /// Cleans the NBT data to something human-readable.
    /// </summary>
    void Clean()
    {
        NAME.Value = Program.Utility.StripSpecial(NAME.Value);
    }
}