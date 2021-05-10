using System.Collections.Generic;

namespace FsInfoCat.Providers
{
    public interface IReadOnlyDictionaryProvider<TKey, TValue>
    {
        IReadOnlyDictionary<TKey, TValue> GetReadOnlyDictionary();
    }
}
