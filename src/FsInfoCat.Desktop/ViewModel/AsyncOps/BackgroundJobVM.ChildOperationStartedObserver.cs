using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        class ChildOperationStartedObserver : IObserver<IAsyncActivity>
        {
            private readonly BackgroundJobVM _target;

            internal ChildOperationStartedObserver([DisallowNull] BackgroundJobVM target) => _target = target ?? throw new ArgumentNullException(nameof(target));

            void IObserver<IAsyncActivity>.OnCompleted()
            {
                _target._activityStartedSubscription?.Dispose();
                _target.Dispatcher.BeginInvoke(() => _target._backingItems.Clear(), DispatcherPriority.Background);
            }

            void IObserver<IAsyncActivity>.OnError([DisallowNull] Exception error)
            {
                if (error is ActivityException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Background progress error observed: Error Code={Code}; Error Message={Message}; Activity={Activity}; StatusMessage={StatusMessage}; Current Operation={CurrentOperation}; Activity ID={ActivityId}",
                        asyncOpError.Code, asyncOpError.Message, asyncOpError.Operation?.ShortDescription, asyncOpError.Operation?.StatusMessage, asyncOpError.Operation?.CurrentOperation, asyncOpError.Operation?.ActivityId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected background progress error observed: Error Message={Message}", error.Message);
            }

            public void OnNext([DisallowNull] IAsyncActivity value) => AppendItem(_target._logger, value, _target.Dispatcher, _target._backingItems);
        }
    }
}
