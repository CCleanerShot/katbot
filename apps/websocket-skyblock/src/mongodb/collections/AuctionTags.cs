using System.Runtime.Serialization;
using Cyotek.Data.Nbt;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuctionTags
{
    [BsonId]
    [DataMember]
    public ObjectId _id { get; set; }
    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name;
    /// <summary>
    /// The type of tag.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public TagType Type = TagType.None;
    /// <summary>
    /// A list of values this tag has been seen with.
    /// </summary>
    public List<string> Values = new List<string>();

    public AuctionTags(string _Name, TagType _Type)
    {
        Name = _Name;
        Type = _Type;
    }
}