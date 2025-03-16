using System.Numerics;
using System.Text.RegularExpressions;
using Cyotek.Data.Nbt;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuctionTag
{
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
    /// The value for this tag.
    /// </summary>
    public string Value;

    public AuctionTag(string _Name, TagType _Type, string _Value)
    {
        Name = _Name;
        Type = _Type;
        Value = _Value;
    }

    public sealed override string ToString()
    {
        return $"({Name}, {Value}) ({Type})";
    }

    public sealed override bool Equals(object? obj)
    {
        if (obj is not AuctionTag attribute)
            return false;

        AuctionTag c1 = this;
        AuctionTag c2 = attribute;

        if (c1.Type != c2.Type)
            return false;

        if (c1.Name != c2.Name)
            return false;

        switch (c1.Type)
        {
            case TagType.Int:
                Regex regex = new Regex($".+ [1-{c1.Value}]");
                return regex.Match(c2.Value).Success;
            default:
                return c1.Value == c2.Value;
        }
    }

    public sealed override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, Value);
    }

    public static bool operator ==(AuctionTag c1, AuctionTag c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(AuctionTag c1, AuctionTag c2)
    {
        return !c1.Equals(c2);
    }
}
