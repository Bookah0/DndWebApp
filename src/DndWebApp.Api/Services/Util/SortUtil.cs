namespace DndWebApp.Api.Services.Util;

public static class SortUtil
{
    public static Dictionary<T, int> CreateOrderLookup<T>(T[] fixedSortOrder) where T : notnull
    {
        return fixedSortOrder
            .Select((name, index) => new { name, index })
            .ToDictionary(x => x.name, x => x.index);
    }

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
}