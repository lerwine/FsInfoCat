using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public class GeneralizableEqualityComparer<T> : IGeneralizableEqualityComparer<T>
    {
        public static readonly IGeneralizableEqualityComparer<T> Default;
        private readonly IEqualityComparer<T> _typedComparer;
        private readonly IEqualityComparer _genericComparer;

        static GeneralizableEqualityComparer()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(string)))
                Default = (IGeneralizableEqualityComparer<T>)Activator.CreateInstance(typeof(GeneralizableEqualityComparer<>).MakeGenericType(type), new object[] { StringComparer.InvariantCulture });
            else
                Default = (IGeneralizableEqualityComparer<T>)Activator.CreateInstance(typeof(GeneralizableEqualityComparer<>).MakeGenericType(type), new object[] { EqualityComparer<T>.Default });
        }

        public GeneralizableEqualityComparer() : this((IEqualityComparer<T>)null) { }

        public GeneralizableEqualityComparer(IComparer comparer)
        {
            if (comparer is null)
                _typedComparer = EqualityComparer<T>.Default;
            else if (comparer is IEqualityComparer<T> eq)
                _typedComparer = eq;
            else if (comparer is IComparer<T> tc)
            {
                _typedComparer = new ComparisonEqualityComparer(tc);
                if (comparer is IEqualityComparer gc)
                {
                    _genericComparer = gc;
                    return;
                }    
            }
            else
            {
                if (comparer is IEqualityComparer g)
                {
                    _genericComparer = g;
                    _typedComparer = new ComparisonEqualityComparer(new GeneralizableComparer<T>(comparer));
                    return;
                }
                else
                    _typedComparer = new ComparisonEqualityComparer(new GeneralizableComparer<T>(comparer));
            }
            if (_typedComparer is IEqualityComparer c)
                _genericComparer = c;
            else
                _genericComparer = new CoersionComparer(_typedComparer);
        }

        public GeneralizableEqualityComparer(IComparer<T> comparer)
        {
            if (comparer is null)
                _typedComparer = EqualityComparer<T>.Default;
            else if (comparer is IEqualityComparer<T> eq)
                _typedComparer = eq;
            else
            {
                _typedComparer = new ComparisonEqualityComparer(comparer);
                if (comparer is IEqualityComparer gc)
                {
                    _genericComparer = gc;
                    return;
                }
            }
            if (_typedComparer is IEqualityComparer c)
                _genericComparer = c;
            else
                _genericComparer = new CoersionComparer(_typedComparer);
        }

        public GeneralizableEqualityComparer(IEqualityComparer comparer)
        {
            if (comparer is null)
            {
                _typedComparer = EqualityComparer<T>.Default;
                _genericComparer = new CoersionComparer(_typedComparer);
            }
            else
            {
                _genericComparer = comparer;
                if (comparer is IEqualityComparer<T> eq)
                    _typedComparer = eq;
                else
                    _typedComparer = new ComparerWrapper(comparer);
            }
        }

        public GeneralizableEqualityComparer(IEqualityComparer<T> comparer)
        {
            if (comparer is null)
                _typedComparer = EqualityComparer<T>.Default;
            else
            {
                _typedComparer = comparer;
                if (comparer is IEqualityComparer eq)
                {
                    _genericComparer = eq;
                    return;
                }
            }
            _genericComparer = new CoersionComparer(_typedComparer);
        }

        public bool Equals(T x, T y) => _typedComparer.Equals(x, y);

        bool IEqualityComparer.Equals(object x, object y) => _genericComparer.Equals(x, y);

        public int GetHashCode(T obj) => _typedComparer.GetHashCode(obj);

        int IEqualityComparer.GetHashCode(object obj) => _genericComparer.GetHashCode(obj);

        private class ComparerWrapper : IEqualityComparer<T>
        {
            private IEqualityComparer _genericComparer;
            internal ComparerWrapper(IEqualityComparer genericComparer) { _genericComparer = genericComparer; }

            public bool Equals(T x, T y) => _genericComparer.Equals(x, y);

            public int GetHashCode(T obj) => _genericComparer.GetHashCode(obj);
        }

        private class ComparisonEqualityComparer : IEqualityComparer<T>
        {
            private readonly IComparer<T> _typedComparer;

            internal ComparisonEqualityComparer(IComparer<T> typedComparer) { _typedComparer = typedComparer; }

            public bool Equals(T x, T y) => _typedComparer.Compare(x, y) == 0;

            public int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
        }

        private class CoersionComparer : IEqualityComparer
        {
            private readonly IEqualityComparer<T> _typedComparer;

            internal CoersionComparer(IEqualityComparer<T> typedComparer) { _typedComparer = typedComparer; }

            public new bool Equals(object x, object y)
            {
                if (Coersion<T>.Default.TryCoerce(x, out T a))
                    return Coersion<T>.Default.TryCoerce(y, out T b) && _typedComparer.Equals(a, b);
                if (Coersion<T>.Default.TryCoerce(y, out _))
                    return false;
                return Comparer.Default.Compare(x, y) == 0;
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
