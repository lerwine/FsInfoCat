using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections.Internal
{
    internal class GeneralizableOrderAndEqualityComparer<T> : IGeneralizableOrderAndEqualityComparer<T>
    {
        private readonly IGeneralizableEqualityComparer<T> _equalityComparer;
        private readonly IGeneralizableComparer<T> _comparer;

        internal GeneralizableOrderAndEqualityComparer(IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
        {
            _equalityComparer = equalityComparer.ToGeneralizable();
            _comparer = comparer.ToGeneralizable();
        }

        public int Compare(T x, T y) => _comparer.Compare(x, y);

        public bool Equals(T x, T y) => _equalityComparer.Equals(x, y);

        public int GetHashCode(T obj) => _equalityComparer.GetHashCode(obj);

        bool IEqualityComparer.Equals(object x, object y) => _equalityComparer.Equals(x, y);

        int IEqualityComparer.GetHashCode(object obj) => _equalityComparer.GetHashCode(obj);

        int IComparer.Compare(object x, object y) => _comparer.Compare(x, y);

        private class CoersionComparer : IComparer
        {
            private readonly IComparer<T> _typedComparer;

            internal CoersionComparer(IComparer<T> typedComparer) { _typedComparer = typedComparer; }

            public int Compare(object x, object y)
            {
                if (Coersion<T>.Default.TryCoerce(x, out T a))
                {
                    if (Coersion<T>.Default.TryCoerce(y, out T b))
                        return _typedComparer.Compare(a, b);
                    return -1;
                }
                if (Coersion<T>.Default.TryCoerce(y, out _))
                    return 1;
                if (x is null)
                    return (y is null) ? 0 : -1;
                if (y is null)
                    return 1;
                if (x.Equals(y))
                    return 0;
                Type t = x.GetType();
                Type u = y.GetType();
                Type c;
                if (t.Equals(u) || t.IsAssignableFrom(u))
                    c = typeof(Comparer<>).MakeGenericType(t);
                else if (u.IsAssignableFrom(t))
                    c = typeof(Comparer<>).MakeGenericType(u);
                else
                    return Comparer.Default.Compare(x, y);
                return (int)c.GetMethod(nameof(Comparer<T>.Compare)).Invoke(c.GetField(nameof(Comparer<T>.Default)).GetValue(null), new object[] { x, y });
            }
        }

        private class CoersionEqualityComparer : IEqualityComparer
        {
            private readonly IEqualityComparer<T> _typedComparer;

            internal CoersionEqualityComparer(IEqualityComparer<T> typedComparer) { _typedComparer = typedComparer; }
            public new bool Equals(object x, object y)
            {
                if (Coersion<T>.Default.TryCoerce(x, out T a))
                    return Coersion<T>.Default.TryCoerce(y, out T b) && _typedComparer.Equals(a, b);
                if (Coersion<T>.Default.TryCoerce(y, out _))
                    return false;
                if (x is null)
                    return y is null;
                return !(y is null) && x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                if (Coersion<T>.Default.TryCoerce(obj, out T t))
                    return _typedComparer.GetHashCode(t);
                return (obj is null) ? 0 : obj.GetHashCode();
            }
        }
    }
}
