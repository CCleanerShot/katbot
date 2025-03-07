using System.Text.Json.Nodes;
using MongoDB.Bson;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

public class PlayerRoutePlayer
{
    public string uuid = "";
    public string displayname = "";
    public string rank = "";
    public string packageRank = "";
    public string newPackageRank = "";
    public string monthlyPackageRank = "";
    public ulong firstLogin = 0;
    public ulong lastLogin = 0;
    public ulong lastLogout = 0;
    public object? stats;
}
