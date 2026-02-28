using System.Linq.Expressions;

namespace Api.Domain.Shared.Utils;

public static class QueryExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string? str, Expression<Func<T, bool>> predicate) 
        => !string.IsNullOrWhiteSpace(str) ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, object? nullable, Expression<Func<T, bool>> predicate) 
        => nullable is not null ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T, C>(this IQueryable<T> query, IEnumerable<C>? col, Expression<Func<T, bool>> predicate) 
        => col is not null && col.Any() ? query.Where(predicate) : query;

    public static Dictionary<T, int> BuildSortOrder<T>(T[] fixedSortOrder) where T : notnull
    {
        return fixedSortOrder
            .Select((name, index) => new { name, index })
            .ToDictionary(x => x.name, x => x.index);
    }

    public static IQueryable<T> OrderByMany<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, object>>> selectors, bool descending) where T : class
    {
        IOrderedQueryable<T> orderQuery = descending 
            ? query.OrderByDescending(selectors.First()) 
            : query.OrderBy(selectors.First());

        for (int i = 1; i < selectors.Count(); i++)
        {
            orderQuery = descending 
                ? orderQuery.ThenByDescending(selectors.ElementAt(i)) 
                : orderQuery.ThenBy(selectors.ElementAt(i));
        }
        return orderQuery;
    }

    public static IEnumerable<T> OrderByFixed<T>(this IEnumerable<T> query, Func<T, string> selector, Dictionary<string, int> orderDict) where T : class
    {
		return query.OrderBy(e => orderDict.GetValueOrDefault(selector(e), int.MaxValue));
    }
	
}