public static class CollectionUtil
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
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
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

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
}
