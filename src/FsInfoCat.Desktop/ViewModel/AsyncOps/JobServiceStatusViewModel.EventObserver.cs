using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobServiceStatusViewModel
    {
        class EventObserver : IObserver<IAsyncActivity>
        {
            private readonly JobServiceStatusViewModel _target;

            internal IDisposable Subscription { get; }

            internal EventObserver([DisallowNull] JobServiceStatusViewModel target, [DisallowNull] IAsyncActivityService backgroundService)
            {
                if (backgroundService is null) throw new ArgumentNullException(nameof(backgroundService));
                _target = target ?? throw new ArgumentNullException(nameof(target));
                Subscription = backgroundService.SubscribeChildActivityStart(this, activeOps =>
                {
                    foreach (IAsyncActivity activity in activeOps)
                        BackgroundJobVM.AppendItem(_target._logger, activity, _target.Dispatcher, _target._backingItems);
                });
            }

            public void OnCompleted() => _target.Dispatcher.BeginInvoke(() => _target._backingItems.Clear());

            public void OnError([DisallowNull] Exception error)
            {
                if (error is ActivityException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Background progress error observed: Error Code={Code}; Error Message={Message}; Activity={Activity}; StatusMessage={StatusMessage}; Current Operation={CurrentOperation}; Activity ID={ActivityId}",
                        asyncOpError.Code, asyncOpError.Message, asyncOpError.Operation?.ShortDescription, asyncOpError.Operation?.StatusMessage, asyncOpError.Operation?.CurrentOperation, asyncOpError.Operation?.ActivityId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected background progress error observed: Error Message={Message}", error.Message);
            }

            public void OnNext([DisallowNull] IAsyncActivity value) => _target.Dispatcher.Invoke(() => BackgroundJobVM.AppendItem(_target._logger, value, _target.Dispatcher, _target._backingItems));
        }
    }
}
