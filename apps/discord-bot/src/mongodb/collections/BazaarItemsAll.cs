using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class BazaarItemsAll
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

    public BazaarItemsAll(string _ID, string _Name)
    {
        ID = _ID;
        Name = _Name;
    }
}