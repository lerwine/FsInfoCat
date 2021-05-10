using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IDictionaryProvider<TKey, TValue>
    {
        IDictionary<TKey, TValue> GetDictionary();
    }
}
