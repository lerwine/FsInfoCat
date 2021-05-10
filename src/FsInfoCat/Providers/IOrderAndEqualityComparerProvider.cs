using FsInfoCat.Collections;

namespace FsInfoCat.Providers
{
    public interface IOrderAndEqualityComparerProvider<T>
    {
        IOrderAndEqualityComparer<T> GetOrderAndEqualityComparer();
    }
}
