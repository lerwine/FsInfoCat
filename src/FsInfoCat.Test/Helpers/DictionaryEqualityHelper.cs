using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test.Helpers
{
    public abstract class DictionaryEqualityHelper<T, K, V, E> : IEqualityComparer<T>
        where T : class, IEnumerable
        where E : class, IEnumerator
    {
        protected DictionaryEqualityHelper()
        {
            ReferenceComparer = ReferenceEqualityHelper<T>.Default;
            ValueComparer = EqualityHelper<V>.Default;
        }

        public IEqualityComparer<T> ReferenceComparer { get; }
        public IEqualityComparer<V> ValueComparer { get; }

        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            if (ReferenceComparer.Equals(x, y))
                return true;
            int count = GetCount(x);
            if (x is null || y is null || count != GetCount(y))
                return false;
            return count == 0 || (WithEnumerator(x, enumerator =>
            {
                while (enumerator.MoveNext())
                {
                    K key = GetKeyAndValue(enumerator, out V a);
                    if (!(TryGetValue(y, key, out V b) && ValueComparer.Equals(a, b)))
                        return false;
                }
                return true;
            }) && WithEnumerator(y, enumerator =>
            {
                while (enumerator.MoveNext())
                {
                    K key = GetKeyAndValue(enumerator, out V a);
                    if (!(TryGetValue(x, key, out V b) && ValueComparer.Equals(a, b)))
                        return false;
                }
                return true;
            }));
        }

        protected abstract K GetKeyAndValue(E enumerator, out V value);
        protected abstract bool WithEnumerator(T dictionary, Func<E, bool> getReturnValue);
        protected abstract bool TryGetValue(T dictionary, K key, out V value);
        protected abstract int GetCount(T dictionary);
        public int GetHashCode(T obj) => ReferenceComparer.GetHashCode(obj);
    }

    public class DictionaryEqualityHelper<T> : DictionaryEqualityHelper<T, object, object, IDictionaryEnumerator>
        where T : class, IDictionary
    {
        public static readonly DictionaryEqualityHelper<T> Default = new DictionaryEqualityHelper<T>();

        protected override int GetCount(T dictionary) => dictionary.Count;

        protected override object GetKeyAndValue(IDictionaryEnumerator enumerator, out object value)
        {
            value = enumerator.Value;
            return enumerator.Key;
        }

        protected override bool TryGetValue(T dictionary, object key, out object value)
        {
            if (dictionary.Contains(key))
            {
                value = dictionary[key];
                return true;
            }
            value = null;
            return false;
        }

        protected override bool WithEnumerator(T dictionary, Func<IDictionaryEnumerator, bool> getReturnValue) => getReturnValue(dictionary.GetEnumerator());
    }

    public class DictionaryEqualityHelper<T, K, V> : DictionaryEqualityHelper<T, K, V, IEnumerator<KeyValuePair<K, V>>>
        where T : class, IDictionary<K, V>
    {
        public static readonly DictionaryEqualityHelper<T, K, V> Default = new DictionaryEqualityHelper<T, K, V>();

        protected override int GetCount(T dictionary) => dictionary.Count;

        protected override K GetKeyAndValue(IEnumerator<KeyValuePair<K, V>> enumerator, out V value)
        {
            value = enumerator.Current.Value;
            return enumerator.Current.Key;
        }

        protected override bool TryGetValue(T dictionary, K key, out V value) => dictionary.TryGetValue(key, out value);

        protected override bool WithEnumerator(T dictionary, Func<IEnumerator<KeyValuePair<K, V>>, bool> getReturnValue)
        {
            using (IEnumerator<KeyValuePair<K, V>> enumerator = dictionary.GetEnumerator())
                return getReturnValue(enumerator);
        }
    }
}
