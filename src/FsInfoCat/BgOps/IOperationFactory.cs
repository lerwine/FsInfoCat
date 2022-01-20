using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IOperationFactory<TTask, TOperation, TProgress, TEvent>
        where TTask : Task
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncOperation<TEvent>
    {
        string GetActivity(out string initialStatusMessage);

        TTask InvokeAsync(TProgress progress);

        TOperation CreateOperation(CancellationTokenSource tokenSource, TTask task, TProgress progress, IObserver<TEvent> observer);
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IOperationNotifyCompleteFactory<TTask, TOperation, TProgress, TEvent, TFinalEvent> : IOperationFactory<TTask, TOperation, TProgress, TEvent>
        where TTask : Task
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TFinalEvent : TEvent
        where TOperation : ICustomAsyncOperation<TEvent>
    {
        string OnRanToCompletion(TOperation operation);

        string OnCanceled(TOperation operation);

        string OnFaulted(Exception fault, TOperation operation);
    }
}
