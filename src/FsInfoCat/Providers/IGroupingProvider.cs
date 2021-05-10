using System.Linq;

namespace FsInfoCat.Providers
{
    public interface IGroupingProvider<TKey, TElement>
    {
        IGrouping<TKey, TElement> GetGrouping();
    }
}
