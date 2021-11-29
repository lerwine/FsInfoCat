using FsInfoCat.AsyncOps;
using FsInfoCat.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobFactoryServiceViewModel
    {
        class ActiveStateObserver : IObserver<bool>
        {
            private readonly JobFactoryServiceViewModel _target;

            internal IDisposable Subscription { get; }

            public ActiveStateObserver([DisallowNull] JobFactoryServiceViewModel target, [DisallowNull] IBackgroundProgressService backgroundService)
            {
                _target = target;
                Subscription = backgroundService.Subscribe(this);
                target.IsBusy = backgroundService.IsActive;
            }

            public void OnCompleted() => _target.Dispatcher.BeginInvoke(() => _target.IsBusy = false);

            public void OnError(Exception error)
            {
                if (error is AsyncOperationException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Active state error observed: Activity={Activity}; Status Message={Message}; Current Operation={CurrentOperation}; Operation ID={OperationId}", asyncOpError.Activity, asyncOpError.Message, asyncOpError.CurrentOperation, asyncOpError.OperationId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected active state error observed");
            }

            public void OnNext(bool value) => _target.Dispatcher.BeginInvoke(() => _target.IsBusy = value);
        }
    }
}
