using System.Runtime.Serialization;
using Cyotek.Data.Nbt;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuctionBuy
{
    [BsonId]
    [DataMember]
    public ObjectId _id { get; set; }
    /// <summary>
    /// The Hypixel ID of the item.
    /// </summary>
    public string ID;
    /// <summary>
    /// The price to start evaluating the threshold for alerts.
    /// </summary>
    public ulong Price;
    /// <summary>
    /// Whether or not to remove the tracked item after successfully found.
    /// </summary>
    public bool RemovedAfter;
    /// <summary>
    /// The Discord ID of the user that submitted the tracking item.
    /// </summary>
    public string UserId;
    /// <summary>
    /// List of 'Name" of the extra attributes of the related item.
    /// </summary>
    public List<string> ExtraAttributes = new List<string>();

    public AuctionBuy(string _ID, ulong _Price, bool _RemovedAfter, string _UserId, List<string>? _ExtraAttributes = null)
    {
        ID = _ID;
        Price = _Price;
        RemovedAfter = _RemovedAfter;
        UserId = _UserId;

        if (_ExtraAttributes != null)
            ExtraAttributes = _ExtraAttributes;
    }
}