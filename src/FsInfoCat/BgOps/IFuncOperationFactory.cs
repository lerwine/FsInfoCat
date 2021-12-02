using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IFuncOperationFactory<TOperation, TProgress, TEvent, TResult> : IOperationFactory<Task<TResult>, TOperation, TProgress, TEvent>
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncProducer<TEvent, TResult>
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IFuncOperationFactory<TOperation, TProgress, TEvent, TResultEvent, TResult> : IFuncOperationFactory<TOperation, TProgress, TEvent, TResult>, IOperationNotifyCompleteFactory<Task<TResult>, TOperation, TProgress, TEvent, TResultEvent>
            where TProgress : IAsyncOpProgress
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : TEvent, IAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>
    {
    }
}
