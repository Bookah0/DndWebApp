using System.Linq.Expressions;

namespace Api.Domain.Shared.Utils;

public static class QueryUtil
{
    public static Dictionary<T, int> CreateOrderLookup<T>(T[] fixedSortOrder) where T : notnull
    {
        return fixedSortOrder
            .Select((name, index) => new { name, index })
            .ToDictionary(x => x.name, x => x.index);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool? boolean, Expression<Func<T, bool>> predicate) 
        => boolean is not null ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, int? i, Expression<Func<T, bool>> predicate) 
        => i is not null ? query.Where(predicate) : query;    

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string? str, Expression<Func<T, bool>> predicate) 
        => !string.IsNullOrWhiteSpace(str) ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, ICollection<string>? col, Expression<Func<T, bool>> predicate) 
        => col?.HasContent() == true ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, ICollection<int>? col, Expression<Func<T, bool>> predicate) 
        => col?.HasContent() == true ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Guid? guid, Expression<Func<T, bool>> predicate) 
      => guid is not null ? query.Where(predicate) : query;
}