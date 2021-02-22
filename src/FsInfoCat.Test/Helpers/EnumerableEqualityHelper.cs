using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test.Helpers
{
    public abstract class EnumerableEqualityHelper<T, V, E> : IEqualityComparer<T>
        where T : class, IEnumerable
        where E : IEnumerator
    {
        internal EnumerableEqualityHelper()
        {
            ReferenceComparer = ReferenceEqualityHelper<T>.Default;
            ValueComparer = EqualityHelper<V>.DefaultNoEnumerate;
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
            return count == 0 || WithEnumerator(x, a => WithEnumerator(y, b =>
            {
                while (a.MoveNext())
                {
                    if (!(b.MoveNext() && ValueComparer.Equals(GetValue(a), GetValue(b))))
                        return false;
                }
                return !b.MoveNext();
            }));
        }

        protected abstract V GetValue(E x);

        public abstract int GetCount(T collection);

        public abstract bool WithEnumerator(T collection, Func<E, bool> getResultFunc);

        public int GetHashCode(T obj) => ReferenceComparer.GetHashCode(obj);
    }

    public class EnumerableEqualityHelper<T> : EnumerableEqualityHelper<T, object, IEnumerator>
        where T : class, ICollection
    {
        public static readonly EnumerableEqualityHelper<T> Default = new EnumerableEqualityHelper<T>();

        public override int GetCount(T collection) => collection.Count;

        public override bool WithEnumerator(T collection, Func<IEnumerator, bool> getResultFunc) => getResultFunc(collection.GetEnumerator());

        protected override object GetValue(IEnumerator x) => x.Current;
    }

    public class EnumerableEqualityHelper<T, V> : EnumerableEqualityHelper<T, V, IEnumerator<V>>
        where T : class, ICollection<V>
    {
        public static readonly EnumerableEqualityHelper<T, V> Default = new EnumerableEqualityHelper<T, V>();

        public override int GetCount(T collection) => collection.Count;

        public override bool WithEnumerator(T collection, Func<IEnumerator<V>, bool> getResultFunc)
        {
            using (IEnumerator<V> enumerator = collection.GetEnumerator())
                return getResultFunc(enumerator);
        }

        protected override V GetValue(IEnumerator<V> x) => x.Current;
    }
}
