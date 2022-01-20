using System;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class AsyncProducer<TState, TResult> : IAsyncProducer<TState, TResult>
    {
        public Task<TResult> Task => throw new NotImplementedException();

        public AsyncOpEventArgs<TState> LastEvent => throw new NotImplementedException();

        public bool IsCancellationRequested => throw new NotImplementedException();

        public IAsyncOpEventArgs ParentOperation => throw new NotImplementedException();

        public TaskStatus Status => throw new NotImplementedException();

        public TState AsyncState => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public string Activity => throw new NotImplementedException();

        public string StatusDescription => throw new NotImplementedException();

        public string CurrentOperation => throw new NotImplementedException();

        IAsyncOpEventArgs ICustomAsyncOperation<IAsyncOpEventArgs>.LastEvent => throw new NotImplementedException();

        IAsyncOpEventArgs<TState> ICustomAsyncOperation<IAsyncOpEventArgs<TState>>.LastEvent => throw new NotImplementedException();

        IAsyncOpEventArgs<TState> IAsyncAction<TState>.LastEvent => throw new NotImplementedException();

        IAsyncOpEventArgs IAsyncAction.LastEvent => throw new NotImplementedException();

        Task IAsyncAction.Task => throw new NotImplementedException();

        IAsyncOpStatus IAsyncOpStatus.ParentOperation => throw new NotImplementedException();

        IAsyncOpInfo IAsyncOpInfo.ParentOperation => throw new NotImplementedException();

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void CancelAfter(int millisecondsDelay)
        {
            throw new NotImplementedException();
        }

        public void CancelAfter(TimeSpan delay)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<AsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }
    }
}
