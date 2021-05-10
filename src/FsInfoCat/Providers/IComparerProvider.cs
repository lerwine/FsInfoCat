using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IComparerProvider<T>
    {
        IComparer<T> GetComparer();
    }
}
