namespace FsInfoCat.Collections
{
    public interface IGeneralizableOrderAndEqualityComparer<T> : IOrderAndEqualityComparer<T>, IGeneralizableEqualityComparer<T>, IGeneralizableComparer<T>
    {

    }
}
