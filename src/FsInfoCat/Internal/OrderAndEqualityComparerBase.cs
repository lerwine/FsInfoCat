using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    internal class OrderAndEqualityComparerBase<T> : EqualityComparerBase<T>, IGeneralizableOrderAndEqualityComparer<T> where T : class
    {
        public int Compare(T x, T y)
        {
            throw new NotImplementedException();
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
