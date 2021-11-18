using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public interface IActionOperationFactory<TOperation, TProgress, TEvent> : IOperationFactory<Task, TOperation, TProgress, TEvent>
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncOperation<TEvent>
    {
    }

    public interface IActionOperationNotifyCompleteFactory<TOperation, TProgress, TEvent> : IActionOperationFactory<TOperation, TProgress, TEvent>, IOperationNotifyCompleteFactory<Task, TOperation, TProgress, TEvent, TEvent>
        where TProgress : IAsyncOpProgress
        where TEvent : IAsyncOpEventArgs
        where TOperation : ICustomAsyncOperation<TEvent>
    {
    }
}
