using System;
using System.Collections.Generic;

namespace CodeGeneration
{
    class ProxyValueComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _compareFunc;
        internal ProxyValueComparer(Func<T, T, int> compareFunc) { _compareFunc = compareFunc; }
        public int Compare(T x, T y) => _compareFunc(x, y);
    }
}
