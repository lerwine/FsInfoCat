using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobServiceStatusViewModel
    {
        class ActiveStateObserver : IObserver<bool>
        {
            private readonly JobServiceStatusViewModel _target;

            internal IDisposable Subscription { get; }

            internal ActiveStateObserver([DisallowNull] JobServiceStatusViewModel target, [DisallowNull] IAsyncActivityService backgroundService)
            {
                _target = target;
                Subscription = backgroundService.Subscribe(this);
                target.IsBusy = backgroundService.IsActive;
            }

            public void OnCompleted() => _target.Dispatcher.BeginInvoke(() => _target.IsBusy = false);

            public void OnError(Exception error)
            {
                if (error is ActivityException activityException)
                    _target._logger.LogError(activityException.Code.ToEventId(), activityException, "Active state error observed: Activity={ShortDescription}; Status Message={Message}; Current Operation={CurrentOperation}; Activity ID={ActivityId}", activityException.Operation?.ShortDescription, activityException.Message, activityException.Operation?.CurrentOperation, activityException.Operation?.ActivityId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected active state error observed");
            }

            public void OnNext(bool value) => _target.Dispatcher.BeginInvoke(() => _target.IsBusy = value);
        }
    }
}
