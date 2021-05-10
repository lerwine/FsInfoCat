using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IReadOnlyCollectionProvider<T>
    {
        ICollection<T> GetReadOnlyCollection();
    }
}
