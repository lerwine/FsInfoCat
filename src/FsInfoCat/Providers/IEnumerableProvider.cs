using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IEnumerableProvider<T>
    {
        IEnumerable<T> GetEnumerable();
    }
}
