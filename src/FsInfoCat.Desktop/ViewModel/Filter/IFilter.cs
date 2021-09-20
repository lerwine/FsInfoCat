using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IFilter : INotifyDataErrorInfo
    {
        event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;
    }
}
