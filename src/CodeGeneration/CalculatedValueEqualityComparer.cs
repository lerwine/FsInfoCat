using System;
using System.Collections.Generic;

namespace CodeGeneration
{
    class CalculatedValueEqualityComparer<T, V> : IEqualityComparer<T>
        where V : class
    {
        private readonly Func<T, V> _getValue;
        private readonly IEqualityComparer<V> _resultComparer;
        internal CalculatedValueEqualityComparer(Func<T, V> getValue, IEqualityComparer<V> resultComparer)
        {
            _getValue = getValue;
            _resultComparer = resultComparer;
        }

        public bool Equals(T x, T y)
        {
            V a = _getValue(x);
            V b = _getValue(y);
            return (a is null) ? b is null : (b is not null && _resultComparer.Equals(a, b));
        }

        public int GetHashCode(T obj)
        {
            V v = _getValue(obj);
            return (v is null) ? 0 : _resultComparer.GetHashCode(v);
        }
    }
}
