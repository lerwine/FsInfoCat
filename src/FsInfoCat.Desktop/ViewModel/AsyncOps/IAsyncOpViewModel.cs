using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public interface IAsyncOpViewModel
    {
        event DependencyPropertyChangedEventHandler AsyncOpStatusPropertyChanged;
        event DependencyPropertyChangedEventHandler StatusMessagePropertyChanged;
        event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;
        event DependencyPropertyChangedEventHandler StartedPropertyChanged;
        event DependencyPropertyChangedEventHandler StoppedPropertyChanged;
        event DependencyPropertyChangedEventHandler DurationPropertyChanged;
        event EventHandler OperationRanToCompletion;
        event EventHandler OperationCanceled;
        event EventHandler<OpFailedEventArgs> OperationFailed;
        Guid ConcurrencyId { get; }
        AsyncOpStatusCode AsyncOpStatus { get; }
        StatusMessageLevel MessageLevel { get; }
        string StatusMessage { get; }
        DateTime? Started { get; }
        DateTime? Stopped { get; }
        TimeSpan? Duration { get; }
        Dispatcher Dispatcher { get; }
        bool CheckAccess();
        void VerifyAccess();
        void Cancel(bool throwOnFirstException);
        void Cancel();
        Task GetTask();
    }
    public interface IAsyncOpViewModel<TState> : IAsyncOpViewModel
    {
        event DependencyPropertyChangedEventHandler StatePropertyChanged;
        TState State { get; }
    }
}
