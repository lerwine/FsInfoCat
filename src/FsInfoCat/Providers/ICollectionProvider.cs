using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface ICollectionProvider<T>
    {
        ICollection<T> GetCollection();
    }
}
