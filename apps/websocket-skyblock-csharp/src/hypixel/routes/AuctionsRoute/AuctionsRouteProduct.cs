using Cyotek.Data.Nbt;
using Newtonsoft.Json.Linq;

public class AuctionsRouteProduct : AuctionsRouteProductMinimal
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

    public string item_bytes
    {
        set
        {
            DOCUMENT = new NbtDocument();

            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));
            DOCUMENT.Load(memoryStream);

            TagCompound ROOT = DOCUMENT.DocumentRoot;

            if (ROOT.Value.Count == 0)
                return;

            TagCompound BASE = ((ROOT.Value[0] as TagList)!.Value[0] as TagCompound)!;
            TagCompound TAG = (BASE.Value["tag"] as TagCompound)!;

            // /tag
            TagCompound EXTRA_ATTRIBUTES = (TAG.Value["ExtraAttributes"] as TagCompound)!;
            TagCompound DISPLAY = (TAG.Value["display"] as TagCompound)!;

            ITEM_ID = (EXTRA_ATTRIBUTES.Value["id"] as TagString)!.Value!;
            ITEM_NAME = Utility.StripSpecial((DISPLAY.Value["Name"] as TagString)!.Value!);

            foreach (Tag tag in EXTRA_ATTRIBUTES.Value)
            {
                if (BANNED_ATTRIBUTES.Contains(tag.Name))
                    continue;

                switch (tag)
                {
                    case TagCompound tagCompound:
                        foreach (Tag innerTag in tagCompound.Value)
                            AuctionTags.Add(new AuctionTag(tag.Name, innerTag.Type, $"{innerTag.Name} {innerTag.GetValue()}"));
                        break;
                    default:
                        AuctionTags.Add(new AuctionTag(tag.Name, tag.Type, $"{tag.GetValue()}"));
                        break;
                }
            }
        }
    }

}
