using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IListProvider<T>
    {
        IList<T> GetList();
    }
}
