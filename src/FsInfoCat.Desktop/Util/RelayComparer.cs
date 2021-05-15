using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Util
{
    public sealed class RelayComparer<TValue, TBase> : IEqualityComparer<TValue>
        where TBase : class
        where TValue : TBase
    {
        public static IEqualityComparer<TValue> Default = new RelayComparer<TValue, TBase>(Extensions.GetComparisonService().GetEqualityComparer<TBase>());

        private readonly IEqualityComparer<TBase> _backingComparer;
        public RelayComparer(IEqualityComparer<TBase> backingComparer)
        {
            _backingComparer = backingComparer ?? throw new ArgumentNullException(nameof(backingComparer));
        }
        public bool Equals(TValue x, TValue y) => _backingComparer.Equals(x, y);
        public int GetHashCode(TValue obj) => _backingComparer.GetHashCode(obj);
    }

}
