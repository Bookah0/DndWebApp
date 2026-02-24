using Api.Infrastructure.Middleware.ExceptionHandling;

namespace Api.Domain.Shared.Utils;

public static class CollectionUtil
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    public static void RemoveMany<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            var removed = collection.Remove(item);

            if(!removed)
                throw new ValidationException("Item to remove not found in collection.");
        }
    }
    
    public static bool HasContent<T>(this ICollection<T>? collection)
    {
        return collection is not null && collection.Count != 0;
    }
}
