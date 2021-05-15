using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Services
{
    public interface ICollectionsService
    {
        ICollection<T> Collection<T>(params T[] items);

        ICollection<T> Collection<T>(IEqualityComparer<T> itemComparer, params T[] item);

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>();

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer);

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer);

        ICollection EmptyGenericCollection();

        IDictionary EmptyGenericDictionary();

        IList EmptyGenericList();

        ICollection<T> EmptyCollection<T>();

        ICollection<T> EmptyCollection<T>(IEqualityComparer<T> itemComparer);

        IList<T> EmptyList<T>();

        IList<T> EmptyList<T>(IEqualityComparer<T> itemComparer);

        IReadOnlyCollection<T> EmptyReadOnlyCollection<T>();

        IReadOnlyCollection<T> EmptyReadOnlyCollection<T>(IEqualityComparer<T> itemComparer);

        IReadOnlyList<T> EmptyReadOnlyList<T>();

        IReadOnlyList<T> EmptyReadOnlyList<T>(IEqualityComparer<T> itemComparer);

        IList<T> List<T>(params T[] items);

        IList<T> List<T>(IEqualityComparer<T> itemComparer, params T[] items);

        IReadOnlyCollection<T> ReadOnlyCollection<T>(params T[] items);

        IReadOnlyCollection<T> ReadOnlyCollection<T>(IEqualityComparer<T> itemComparer, params T[] items);

        IReadOnlyList<T> ReadOnlyList<T>(params T[] items);

        IReadOnlyList<T> ReadOnlyList<T>(IEqualityComparer<T> itemComparer, params T[] items);
    }
}
