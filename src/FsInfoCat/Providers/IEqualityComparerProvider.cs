using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IEqualityComparerProvider<T>
    {
        IEqualityComparer<T> GetEqualityComparer();
    }
}
