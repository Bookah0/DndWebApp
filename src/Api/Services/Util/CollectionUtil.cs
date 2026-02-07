using Api.Middlewares.ExceptionHandling;

namespace Api.Services.Util;

// Collection methods for ICollection<T> that are not provided by default, such as AddRange, RemoveMany, etc.
// Mostly to prevent creating unnessecary lists and casting
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

    public static void RemoveFirst<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                collection.Remove(item);
                return;
            }
        }
    }

    public static void RemoveAt<T>(this ICollection<T> collection, int index)
    {
        if (collection is IList<T> list)
        {
            list.RemoveAt(index);
            return;
        } 
        else
        {
            if (index < 0 || index >= collection.Count)
                throw new ValidationException($"Index {index} is out of range for collection of size {collection.Count}.");

            int currentIndex = 0;
            using var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (currentIndex == index)
                {
                    collection.Remove(enumerator.Current);
                    return;
                }
                currentIndex++;
            }
        }
    }

    public static bool HasDuplicates<T>(this ICollection<T> collection)
    {
        var seenSet = new HashSet<T>();
        foreach (var item in collection)
        {
            if (!seenSet.Add(item))
                throw new ValidationException($"Duplicate item found in collection: {item}");
        }
        return false;
    }

    public static void RemoveDuplicates<T>(this ICollection<T> collection)
    {
        var seenSet = new HashSet<T>();
        var itemsToRemove = new List<T>();

        foreach (var item in collection)
        {
            if (!seenSet.Add(item))
                itemsToRemove.Add(item);
        }

        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }
    }
}
