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
        class EventObserver : IObserver<IBackgroundProgressEvent>
        {
            private readonly JobFactoryServiceViewModel _target;

            internal IDisposable Subscription { get; }

            public EventObserver([DisallowNull] JobFactoryServiceViewModel target, [DisallowNull] IBackgroundProgressService backgroundService)
            {
                _target = target;
                Subscription = backgroundService.Subscribe(this, activeOps =>
                {
                    foreach (IBackgroundOperation backgroundOperation in activeOps)
                        _target.OnOperationStarted(backgroundOperation, null, backgroundOperation);
                });
                target.IsBusy = backgroundService.IsActive;
            }

            public void OnCompleted() => _target.Dispatcher.BeginInvoke(() => _target._backingItems.Clear());

            public void OnError(Exception error)
            {
                if (error is AsyncOperationException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Background progress error observed: Activity={Activity}; Status Message={Message}; Current Operation={CurrentOperation}; Operation ID={OperationId}", asyncOpError.Activity, asyncOpError.Message, asyncOpError.CurrentOperation, asyncOpError.OperationId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected background progress error observed");
            }

            public void OnNext(IBackgroundProgressEvent value)
            {
                if (value is IBackgroundProgressStartedEvent startedEvent)
                    _target.Dispatcher.BeginInvoke(() => _target.OnOperationStarted(startedEvent, startedEvent.Code, startedEvent));
            }
        }
    }
}
