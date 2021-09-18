using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IFilter : INotifyDataErrorInfo
    {
        event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;
    }
}
