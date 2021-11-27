using System;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public interface IBackgroundFunc<TResult> : IBackgroundOperation, IObservable<IBackgroundProgressEvent>
    {
        new Task<TResult> Task { get; }
    }

    public interface IBackgroundFunc<TState, TResult> : IBackgroundFunc<TResult>, IBackgroundOperation<TState>, IObservable<IBackgroundProgressEvent<TState>>
    {
    }
}
