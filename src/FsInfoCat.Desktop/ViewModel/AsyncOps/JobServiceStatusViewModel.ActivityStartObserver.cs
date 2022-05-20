using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobServiceStatusViewModel
    {
        class ActivityStartObserver : IObserver<IAsyncActivity>
        {
            private readonly JobServiceStatusViewModel _target;

            internal IDisposable Subscription { get; }

            internal ActivityStartObserver([DisallowNull] JobServiceStatusViewModel target, [DisallowNull] IAsyncActivityService backgroundService)
            {
                if (backgroundService is null) throw new ArgumentNullException(nameof(backgroundService));
                _target = target ?? throw new ArgumentNullException(nameof(target));
                Subscription = backgroundService.SubscribeChildActivityStart(this, activeOps =>
                {
                    foreach (IAsyncActivity activity in activeOps)
                        BackgroundJobVM.OnActivityStarted(_target._logger, activity, _target.Dispatcher, _target._backingItems);
                });
            }

            void IObserver<IAsyncActivity>.OnCompleted()
            {
                _target._logger.LogDebug($"{nameof(JobServiceStatusViewModel)}.{nameof(ActivityStartObserver)}.{nameof(IObserver<IAsyncActivity>.OnCompleted)} invoked");
                _target.Dispatcher.BeginInvoke(() => _target._backingItems.Clear());
            }

            void IObserver<IAsyncActivity>.OnError([DisallowNull] Exception error)
            {
                if (error is ActivityException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Background progress error observed: Error Code={Code}; Error Message={Message}; Activity={Activity}; StatusMessage={StatusMessage}; Current Operation={CurrentOperation}; Activity ID={ActivityId}",
                        asyncOpError.Code, asyncOpError.Message, asyncOpError.Operation?.ShortDescription, asyncOpError.Operation?.StatusMessage, asyncOpError.Operation?.CurrentOperation, asyncOpError.Operation?.ActivityId);
                else
                    _target._logger.LogError(Model.ErrorCode.Unexpected.ToEventId(), error, "Unexpected background progress error observed: Error Message={Message}", error.Message);
            }

            void IObserver<IAsyncActivity>.OnNext([DisallowNull] IAsyncActivity value)
            {
                _target._logger.LogDebug(@$"{nameof(JobServiceStatusViewModel)}.{nameof(ActivityStartObserver)}.{nameof(IObserver<IAsyncActivity>.OnNext)} invoked: ActivityId={{ActivityId}};
ShortDescription={{ShortDescription}}
StatusMessage={{StatusMessage}}",
                    value.ActivityId, value.ShortDescription, value.StatusMessage);
                _target.Dispatcher.Invoke(() => BackgroundJobVM.OnActivityStarted(_target._logger, value, _target.Dispatcher, _target._backingItems));
            }
        }
    }
}
