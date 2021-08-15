using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    internal interface IAsyncOpManagerViewModel
    {
        event DependencyPropertyChangedEventHandler IsBusyPropertyChanged;
        Dispatcher Dispatcher { get; }
        bool CheckAccess();
        void VerifyAccess();
        void CancelAll(bool throwOnFirstException);
        void CancelAll();
        bool RemoveOperation(IAsyncOpViewModel item);
    }
    internal interface IAsyncOpManagerViewModel<TState> : IAsyncOpManagerViewModel
    {
        bool RemoveOperation(IAsyncOpViewModel<TState> item);
    }
}
