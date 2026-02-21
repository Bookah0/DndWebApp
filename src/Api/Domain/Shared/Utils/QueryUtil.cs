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

    public static ICollection<T> OrderByMany<T>(ICollection<T> toOrder, IEnumerable<Func<T, object>> selectors, bool descending)
    {
        var selectorList = selectors.ToList();
        var query = descending ? toOrder.OrderByDescending(selectorList[0]) : toOrder.OrderBy(selectorList[0]);

        for (int i = 1; i < selectors.Count(); i++)
        {
            query = descending ? query.ThenByDescending(selectorList[i]) : query.ThenBy(selectorList[i]);
        }
        return [.. query];
    }

    public static IQueryable<T> OrderByMany<T>(IQueryable<T> query, IEnumerable<Func<T, object>> selectors, bool descending) where T : class
    {
        var selectorList = selectors.ToList();
        var orderQuery = descending 
            ? query.OrderByDescending(selectors.ElementAt(0)) 
            : query.OrderBy(selectors.ElementAt(0));

        for (int i = 1; i < selectors.Count(); i++)
        {
            orderQuery = descending 
                ? orderQuery.ThenByDescending(selectors.ElementAt(i)) 
                : orderQuery.ThenBy(selectors.ElementAt(i));
        }
        return query;
    }
}