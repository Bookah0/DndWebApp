using System.Linq.Expressions;

namespace Api.Domain.Shared.Utils;

public static class QueryUtil
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string? str, Expression<Func<T, bool>> predicate) 
        => !string.IsNullOrWhiteSpace(str) ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, ICollection<object>? col, Expression<Func<T, bool>> predicate) 
        => col?.HasContent() == true ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, object? nullable, Expression<Func<T, bool>> predicate) 
        => nullable is not null ? query.Where(predicate) : query;
}