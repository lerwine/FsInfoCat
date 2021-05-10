using System.Linq;

namespace FsInfoCat.Providers
{
    public interface ILookupProvider<TKey, TElement>
    {
        ILookup<TKey, TElement> GetLookup();
    }
}
