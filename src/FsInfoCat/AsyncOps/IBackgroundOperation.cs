using System;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundOperation : ICancellableOperation, IDisposable
    {
        Task Task { get; }
    }

    public interface IBackgroundOperation<TState> : IBackgroundOperation, ICancellableOperation<TState>
    {
    }
}
