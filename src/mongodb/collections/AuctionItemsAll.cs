using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuctionItemsAll
{
    [BsonId]
    [DataMember]
    public ObjectId _id { get; set; }
    /// <summary>
    /// The Hypixel ID of the item.
    /// </summary>
    public string ID;
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string Name;
    /// <summary>
    /// List of "Name" of the extra attributes that this item has been seen with.
    /// </summary>
    public List<string> ExtraAttributes = new List<string>();

    public AuctionItemsAll(string _ID, string _Name, List<string>? _ExtraAttributes = null)
    {
        ID = _ID;
        Name = _Name;

        if (_ExtraAttributes != null)
            ExtraAttributes = _ExtraAttributes;
    }
}