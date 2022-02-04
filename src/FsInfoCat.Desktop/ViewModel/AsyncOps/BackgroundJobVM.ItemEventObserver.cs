using FsInfoCat.Activities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        /// <summary>
        /// Event observer for <see cref="BackgroundJobVM"/> items associated with an <see cref="IAsyncAction{TEvent}" /> activity.
        /// </summary>
        /// <typeparam name="TEvent">The type of the t event.</typeparam>
        /// <seealso cref="IObserver{TEvent}" />
        internal class ItemEventObserver<TEvent> : IObserver<TEvent>
            where TEvent : class, IActivityEvent
        {
            private readonly Action _onCompleted;
            private DispatcherOperation _currentUiOperation;
            private TEvent _latestEvent;
            private readonly object _syncRoot = new();

            /// <summary>
            /// Gets the target view model.
            /// </summary>
            /// <value>The target view model object.</value>
            protected BackgroundJobVM Target { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemEventObserver{TEvent}"/> class.
            /// </summary>
            /// <param name="target">The target view model.</param>
            /// <param name="onCompleted">The action to be invoked when <see cref="IObserver{TEvent}.OnCompleted"/> is called.</param>
            internal ItemEventObserver([DisallowNull] BackgroundJobVM target, [DisallowNull] Action onCompleted) => (Target, _onCompleted) = (target, onCompleted);

            /// <summary>
            /// Called from the <see cref="Dispatcher"/> thread to update the <see cref="Target"/> view model.
            /// </summary>
            /// <param name="activityEvent">The activity event.</param>
            protected virtual void OnNextEvent([DisallowNull] TEvent activityEvent)
            {
                Target.StatusMessage = activityEvent.StatusMessage;
                if (activityEvent is IOperationEvent operationEvent)
                {
                    Target.CurrentOperation = operationEvent.CurrentOperation;
                    int p = operationEvent.PercentComplete;
                    Target.PercentComplete = (p < 0) ? null : p;
                }
                else
                    Target.CurrentOperation = "";
            }

            /// <summary>
            /// Invoked on the UI <see cref="Dispatcher"/> thread to update the <see cref="Target"/> view model.
            /// </summary>
            private void OnNext()
            {
                TEvent activityEvent;
                Monitor.Enter(_syncRoot);
                try
                {
                    activityEvent = _latestEvent;
                    _latestEvent = null;
                    _currentUiOperation = null;
                }
                finally { Monitor.Exit(_syncRoot); }
                if (activityEvent is not null)
                    OnNextEvent(activityEvent);
            }

            void IObserver<TEvent>.OnCompleted() => _onCompleted?.Invoke();

            void IObserver<TEvent>.OnError(Exception error)
            {
                if (error is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                    error = aggregateException.InnerExceptions[0];
                if (error is ActivityException asyncOpException)
                {
                    string currentOperation = asyncOpException.Operation?.CurrentOperation;
                    if (asyncOpException.Code.TryGetDescription(out string codeDescription))
                        Target.Dispatcher.Invoke(() =>
                        {
                            string statusMessage = asyncOpException.Operation?.StatusMessage;
                            string activity = asyncOpException.Operation?.ShortDescription;
                            if (string.IsNullOrWhiteSpace(currentOperation))
                                MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Operation?.ShortDescription}
Status Message: {asyncOpException.Operation?.StatusMessage}
Error Type: {asyncOpException.Code.GetDisplayName()} ({codeDescription})
Message: {asyncOpException.Message}", caption: "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                                MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Operation?.ShortDescription}
Error Type: {asyncOpException.Code.GetDisplayName()} ({codeDescription})
Message: {asyncOpException.Message}
Current Operation: {currentOperation}", caption: "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Target.MessageLevel = StatusMessageLevel.Error;
                        });
                    else if (string.IsNullOrWhiteSpace(currentOperation))
                        Target.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Operation?.ShortDescription}
Status Message: {asyncOpException.Operation?.StatusMessage}
Error Type: {asyncOpException.Code.GetDisplayName()}
Message: {asyncOpException.Message}", caption: "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Target.MessageLevel = StatusMessageLevel.Error;
                        });
                    else
                        Target.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {asyncOpException.Operation?.ShortDescription}
Status Message: {asyncOpException.Operation?.StatusMessage}
Error Type: {asyncOpException.Code.GetDisplayName()}
Message: {asyncOpException.Message}
Current Operation: {currentOperation}", caption: "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Target.MessageLevel = StatusMessageLevel.Error;
                        });
                }
                else
                    Target.Dispatcher.Invoke(() =>
                    {
                        string currentOperation = Target.CurrentOperation;
                        if (string.IsNullOrWhiteSpace(currentOperation))
                            MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {Target.ShortDescription}
Latest Status: {Target.StatusMessage}
Message: {error.Message}", caption: "Unexpected Error", MessageBoxButton.OK, (error is WarningException) ? MessageBoxImage.Warning : MessageBoxImage.Error);
                        else MessageBox.Show(Application.Current.MainWindow, messageBoxText: $@"An unexpected has occurred:
Activity: {Target.ShortDescription}
Latest Status: {Target.StatusMessage}
Message: {error.Message}
Current Operation: {currentOperation}", caption: "Unexpected Error", MessageBoxButton.OK, (error is WarningException) ? MessageBoxImage.Warning : MessageBoxImage.Error);
                    });
            }

            void IObserver<TEvent>.OnNext(TEvent value)
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    _latestEvent = value;
                    if (_currentUiOperation is null)
                        _currentUiOperation = Target.Dispatcher.InvokeAsync(OnNext, DispatcherPriority.Background, Target.Token);
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        /// <summary>
        /// Event observer for <see cref="BackgroundJobVM"/> items associated with an <see cref="IAsyncAction{IActivityEvent}" /> activity.
        /// </summary>
        /// <seealso cref="ItemEventObserver{IActivityEvent}" />
        internal class ItemEventObserver : ItemEventObserver<IActivityEvent>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ItemEventObserver"/> class.
            /// </summary>
            /// <param name="target">The target view model.</param>
            /// <param name="onCompleted">The action to be invoked when <see cref="IObserver{IActivityEvent}.OnCompleted"/> is called.</param>
            internal ItemEventObserver([DisallowNull] BackgroundJobVM target, [DisallowNull] Action onCompleted) : base(target, onCompleted) { }
        }
    }
}
