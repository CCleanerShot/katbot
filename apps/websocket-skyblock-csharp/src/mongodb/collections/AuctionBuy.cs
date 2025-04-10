using System.Collections;
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
    /// List of AuctionTags that the tracked item should have.
    /// </summary>
    public List<AuctionTag> AuctionTags = new List<AuctionTag>();
    /// <summary>
    /// The Hypixel ID of the item.
    /// </summary>
    public string ID = "";
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string Name = "";
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

    public sealed override bool Equals(object? obj)
    {
        if (obj is not AuctionBuy buy)
            return false;

        AuctionBuy c1 = this;
        AuctionBuy c2 = buy;

        if (c1.ID != c2.ID)
            return false;

        if (c1.Name != c2.Name)
            return false;

        if (c1.Price != c2.Price)
            return false;

        if (c1.RemovedAfter != c2.RemovedAfter)
            return false;

        if (c1.UserId != c2.UserId)
            return false;

        foreach (AuctionTag tag in c1.AuctionTags)
            if (c2.AuctionTags.Where(e => tag == e).Count() == 0)
                return false;

        return true;
    }

    public sealed override int GetHashCode()
    {
        return HashCode.Combine(_id.CreationTime, ID, Price, RemovedAfter, UserId);
    }

    public static bool operator ==(AuctionBuy c1, AuctionBuy c2) => c1.Equals(c2);
    public static bool operator !=(AuctionBuy c1, AuctionBuy c2) => !c1.Equals(c2);
}