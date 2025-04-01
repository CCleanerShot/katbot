using MongoDB.Driver;
using System.Threading.Tasks;

public static class MongoBotExtensions
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
}