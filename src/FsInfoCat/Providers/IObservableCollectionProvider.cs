using System.Collections.ObjectModel;

namespace FsInfoCat.Providers
{
    public interface IObservableCollectionProvider<T>
    {
        ObservableCollection<T> GetObservableCollection();
    }
}
