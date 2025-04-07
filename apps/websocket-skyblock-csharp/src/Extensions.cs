using MongoDB.Driver;
using System.Net;
using System.Threading.Tasks;

public static class Extensions
{
    public static async Task<List<T>> FindList<T>(this IMongoCollection<T> origin, FilterDefinition<T> filter, FindOptions<T, T>? options = null)
    {
        IAsyncCursor<T>? results = await origin.FindAsync(filter, options);

        if (results == null)
        {
            Utility.Log(Enums.LogLevel.WARN, $"Unexpected null when finding items in {origin.CollectionNamespace}");
            return new List<T>();
        }

        return results.ToList();
    }

    public static async Task<List<T>> FindList<T>(this IMongoCollection<T> origin, System.Linq.Expressions.Expression<Func<T, bool>> filter, FindOptions<T, T>? options = null)
    {
        IAsyncCursor<T>? results = await origin.FindAsync(filter, options);

        if (results == null)
        {
            Utility.Log(Enums.LogLevel.WARN, $"Unexpected null when finding items in {origin.CollectionNamespace}");
            return new List<T>();
        }

        return results.ToList();
    }

    /// <summary>
    /// Extension method for parsing the request as string. Decided not to completely override, and may cause side-effects to internal packages (somehow).
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static string AsString(this HttpListenerRequest origin)
    {
        string result = "";

        result += $"Content Type: {origin.ContentType}\n";

        result += $"Accept Types: [";
        if (origin.AcceptTypes != null)
            foreach (string acceptType in origin.AcceptTypes)
                result += $"{acceptType}|";
        result += "]";

        result += $"Cookies: [";
        foreach (string cookie in origin.Cookies)
            result += $"{cookie}|";
        result += "]";

        result += $"Headers: [";
        foreach (string header in origin.Headers)
            result += $"{header}|";
        result += "]";

        return result;
    }
}