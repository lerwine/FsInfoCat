using FsInfoCat.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Internal
{
    internal class CollectionsService : ICollectionsService
    {
        public ICollection<T> Collection<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Collection<T>(IEqualityComparer<T> itemComparer, params T[] item)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> EmptyCollection<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> EmptyCollection<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer)
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            throw new NotImplementedException();
        }

        public ICollection EmptyGenericCollection()
        {
            throw new NotImplementedException();
        }

        public IDictionary EmptyGenericDictionary()
        {
            throw new NotImplementedException();
        }

        public IList EmptyGenericList()
        {
            throw new NotImplementedException();
        }

        public IList<T> EmptyList<T>()
        {
            throw new NotImplementedException();
        }

        public IList<T> EmptyList<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> EmptyReadOnlyCollection<T>()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> EmptyReadOnlyCollection<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> EmptyReadOnlyList<T>()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> EmptyReadOnlyList<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IList<T> List<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IList<T> List<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> ReadOnlyCollection<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> ReadOnlyCollection<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> ReadOnlyList<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> ReadOnlyList<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}
