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
    public TagType Type = TagType.None;
    /// <summary>
    /// Is a list of "Name" that point to it's tag. Used for compound tags
    /// </summary>
    public List<string> Tags = new List<string>();
    /// <summary>
    /// A list of values this tag has been seen with.
    /// </summary>
    public List<string> Values = new List<string>();

    public AuctionTags(string _Name, TagType _Type, string _Value, List<string>? _Tags = null)
    {
        Name = _Name;
        Type = _Type;
        Values.Add(_Value);

        if (_Tags != null)
            Tags = _Tags;
    }
}