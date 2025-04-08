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

        string[] c1Split = c1.Value.Split(" ", 2);
        string[] c2Split = c2.Value.Split(" ", 2);
        bool wasCompound;

        if (c1Split.Length > 1)
            wasCompound = true;
        else
            wasCompound = false;

        // exceptions
        switch (c1.Name)
        {
            default:
                break;
        }

        // compounds
        if (wasCompound)
            switch (c1.Type)
            {
                case TagType.Int:
                default:
                    return c1Split[0] == c2Split[0] && c1Split[1] == c2Split[1];
            }

        // normal
        else
            switch (c1.Type)
            {
                case TagType.Int:
                default:
                    return c1.Value == c2.Value;
            }
    }

    bool GreaterThan(object? obj)
    {
        if (obj is not AuctionTag attribute)
            return false;

        AuctionTag c1 = this;
        AuctionTag c2 = attribute;

        if (c1.Type != c2.Type)
            throw new Exception("Cannot compare if 2 tags are lesser/greater than each other if they are not similar!");

        if (c1.Name != c2.Name)
            throw new Exception("Cannot compare if 2 tags are lesser/greater than each other if they are not similar!");

        string[] c1Split = c1.Value.Split(" ", 2);
        string[] c2Split = c2.Value.Split(" ", 2);
        bool wasCompound;

        if (c1Split.Length > 1)
            wasCompound = true;
        else
            wasCompound = false;

        // exceptions
        switch (c1.Name)
        {
            default:
                break;
        }

        // compounds
        if (wasCompound)
            switch (c1.Type)
            {
                case TagType.Int:
                default:
                    return c1Split[0] == c2Split[0] && Int32.Parse(c1Split[1]) > Int32.Parse(c2Split[1]);
            }

        // normal
        else
            switch (c1.Type)
            {
                case TagType.Int:
                default:
                    return Int32.Parse(c1.Value) > Int32.Parse(c2.Value);
            }
    }

    public sealed override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, Value);
    }

    public static bool operator ==(AuctionTag c1, AuctionTag c2) => c1.Equals(c2);
    public static bool operator !=(AuctionTag c1, AuctionTag c2) => !c1.Equals(c2);
    public static bool operator >(AuctionTag c1, AuctionTag c2) => c1.GreaterThan(c2);
    public static bool operator <(AuctionTag c1, AuctionTag c2) => !c1.GreaterThan(c2);
    public static bool operator >=(AuctionTag c1, AuctionTag c2) => c1.GreaterThan(c2) || c1.Equals(c2);
    public static bool operator <=(AuctionTag c1, AuctionTag c2) => !c1.GreaterThan(c2) || c1.Equals(c2);
}
