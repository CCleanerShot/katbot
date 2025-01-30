using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class BazaarTracked
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }
    /// <summary>
    /// Name of the item.
    /// </summary>
    public string Name = "";
    /// <summary>
    /// The cost threshold at which the discord will be notified of the item's price change.
    /// </summary>
    public ulong CostThreshold;
    /// <summary>
    /// The total threshold above the cost threshold before notifying.
    /// </summary>
    public ulong TotalThreshold;
}