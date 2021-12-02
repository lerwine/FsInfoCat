using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncAction : IObservable<IBgStatusEventArgs>, IBgActivityObject, IAsyncResult
    {
        Task Task { get; }

        void Cancel();

        void CancelAfter(long millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncAction<TState> : IAsyncAction, IBgActivityObject<TState>, IObservable<IBgStatusEventArgs<TState>>
    {
    }
}
