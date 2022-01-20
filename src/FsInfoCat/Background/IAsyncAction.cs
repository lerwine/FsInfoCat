using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncAction : IObservable<IBgStatusEventArgs>, IBgActivityObject, IAsyncResult
    {
        Task Task { get; }

        void Cancel();

        void CancelAfter(long millisecondsDelay);

        void CancelAfter(TimeSpan delay);
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncAction<TState> : IAsyncAction, IBgActivityObject<TState>, IObservable<IBgStatusEventArgs<TState>>
    {
    }
}
