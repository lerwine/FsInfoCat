using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IReadOnlyListProvider<T>
    {
        IList<T> GetReadOnlyList();
    }
}
