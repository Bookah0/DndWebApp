using Api.Domain.Shared.Enums;
using Api.Domain.Spells.Models;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Infrastructure.Validation;

namespace Api.Domain.Shared.Utils;

public static class SortUtils
{
    public static IQueryable<T> SortBy<T>(
        this IQueryable<T> query, 
        string? sortBy, 
        Dictionary<string, IEnumerable<Func<T, object>>> sortSelectorsMap,
        string defaultSort,
        bool descending = false) 
        where T : class
    {
        if(sortBy is null)
            return query.OrderByMany(sortSelectorsMap[defaultSort], descending);
        
        return query.OrderByMany(sortSelectorsMap![sortBy], descending);
    }

    public static IQueryable<T> OrderByMany<T>(this IQueryable<T> query, IEnumerable<Func<T, object>> selectors, bool descending) where T : class
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
        return (IQueryable<T>)orderQuery;
    }
    

}