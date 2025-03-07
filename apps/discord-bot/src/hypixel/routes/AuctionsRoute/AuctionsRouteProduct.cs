using Cyotek.Data.Nbt;
using Newtonsoft.Json.Linq;

public class AuctionsRouteProduct
{
    static List<string> BANNED_ATTRIBUTES = new List<string>()
    {
        "bossId",
        "builder's_wand_data",
        // "date",
        "id",
        // "PERSONAL_DELETOR_ACTIVE",
        "spawnedFor",
        "recipient_id",
        // "recipient_name",
        // "tickets",
        "timestamp",
        "uuid",
    };
    static NbtDocument DOCUMENT = new NbtDocument();


    #region API_PROPERTIES ordered by API response order
    public string uuid = default!;
    public string auctioneer = default!;
    public ulong starting_bid = default!;
    public ulong highest_bid_amount = default!;
    #endregion

    public string ITEM_ID = default!;
    public string ITEM_NAME = default!;
    public List<ExtraAttribute> ExtraAttributes = new List<ExtraAttribute>();

    public string item_bytes
    {
        set
        {
            DOCUMENT = new NbtDocument();

            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));
            DOCUMENT.Load(memoryStream);

            TagCompound ROOT = DOCUMENT.DocumentRoot;
            TagCompound BASE = ((ROOT.Value[0] as TagList)!.Value[0] as TagCompound)!;
            TagCompound TAG = (BASE.Value["tag"] as TagCompound)!;

            // /tag
            TagCompound EXTRA_ATTRIBUTES = (TAG.Value["ExtraAttributes"] as TagCompound)!;
            TagCompound DISPLAY = (TAG.Value["display"] as TagCompound)!;

            ITEM_ID = (EXTRA_ATTRIBUTES.Value["id"] as TagString)!.Value!;
            ITEM_NAME = Program.Utility.StripSpecial((DISPLAY.Value["Name"] as TagString)!.Value!);

            foreach (Tag tag in EXTRA_ATTRIBUTES.Value)
            {
                if (BANNED_ATTRIBUTES.Contains(tag.Name))
                    continue;

                switch (tag)
                {
                    case TagCompound tagCompound:
                        foreach (Tag innerTag in tagCompound.Value)
                            ExtraAttributes.Add(new ExtraAttribute(tag.Name, innerTag.Type, $"{innerTag.Name} {innerTag.GetValue()}"));
                        break;
                    default:
                        ExtraAttributes.Add(new ExtraAttribute(tag.Name, tag.Type, $"{tag.GetValue()}"));
                        break;
                }
            }
        }
    }

}
