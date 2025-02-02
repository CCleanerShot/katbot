using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class BazaarItem
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// Hypixel ID of the item.
    /// </summary>
    public string ID = "";
    /// <summary>
    /// Name of the item.
    /// </summary>
    public string Name = "";
    /// <summary>
    /// The type of order the watch is, from.
    /// </summary>
    public Enums.OrderType OrderType;
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
    public ulong UserId;
}