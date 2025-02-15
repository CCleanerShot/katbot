using System.Collections;
using System.Runtime.Serialization;
using Cyotek.Data.Nbt;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;



public class AuctionBuy
{
    public class ExtraAttribute
    {
        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name;
        /// <summary>
        /// The value for this tag.
        /// </summary>
        public string Value;

        public ExtraAttribute(string _Name, string _Value)
        {
            Name = _Name;
            Value = _Value;
        }
    }

    [BsonId]
    [DataMember]
    public ObjectId _id { get; set; }
    /// <summary>
    /// List of ExtraAttributes that the tracked item should have.
    /// </summary>
    public List<ExtraAttribute> ExtraAttributes = new List<ExtraAttribute>();
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
}