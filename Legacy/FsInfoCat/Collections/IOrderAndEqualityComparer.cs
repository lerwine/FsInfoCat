using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public class OrderAndEqualityComparer<T> : IOrderAndEqualityComparer<T>
    {
        public int Compare(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IOrderAndEqualityComparer<T> : IEqualityComparer<T>, IComparer<T> { }
}
