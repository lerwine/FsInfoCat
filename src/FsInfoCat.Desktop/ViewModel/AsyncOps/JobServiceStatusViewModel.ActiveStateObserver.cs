using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

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

            void IObserver<bool>.OnCompleted()
            {
                _target._logger.LogDebug($"{nameof(JobServiceStatusViewModel)}.{nameof(ActiveStateObserver)}.{nameof(IObserver<bool>.OnCompleted)} invoked");
                _target.Dispatcher.BeginInvoke(() => _target.IsBusy = false);
            }

            void IObserver<bool>.OnError(Exception error)
            {
                if (error is ActivityException activityException)
                    _target._logger.LogError(activityException.Code.ToEventId(), activityException, "Active state error observed: Activity={ShortDescription}; Status Message={Message}; Current Operation={CurrentOperation}; Activity ID={ActivityId}", activityException.Operation?.ShortDescription, activityException.Message, activityException.Operation?.CurrentOperation, activityException.Operation?.ActivityId);
                else
                    _target._logger.LogError(Model.ErrorCode.Unexpected.ToEventId(), error, "Unexpected active state error observed");
            }

            void IObserver<bool>.OnNext(bool value)
            {
                _target._logger.LogDebug($"{nameof(JobServiceStatusViewModel)}.{nameof(ActiveStateObserver)}.{nameof(IObserver<bool>.OnNext)} invoked; value = {{value}}", value);
                _target.Dispatcher.BeginInvoke(() => _target.IsBusy = value);
            }
        }
    }
}
