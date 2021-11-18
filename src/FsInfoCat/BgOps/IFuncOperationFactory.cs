using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public interface IFuncOperationFactory<TOperation, TProgress, TEvent, TResult> : IOperationFactory<Task<TResult>, TOperation, TProgress, TEvent>
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncProducer<TEvent, TResult>
    {
    }

    public interface IFuncOperationFactory<TOperation, TProgress, TEvent, TResultEvent, TResult> : IFuncOperationFactory<TOperation, TProgress, TEvent, TResult>, IOperationNotifyCompleteFactory<Task<TResult>, TOperation, TProgress, TEvent, TResultEvent>
            where TProgress : IAsyncOpProgress
            where TEvent : IAsyncOpEventArgs
            where TResultEvent : TEvent, IAsyncOpResultArgs<TResult>
            where TOperation : ICustomAsyncProducer<TEvent, TResult>
    {
    }
}
