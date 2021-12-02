using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class FuncOperationFactory<TState, TResult> : IFuncOperationFactory<AsyncProducer<TState, TResult>, AsyncOpProgress<TState>, AsyncOpEventArgs<TState>, AsyncResultEventArgs<TState, TResult>, TResult>
    {
        private string activity;
        private string initialStatusMessage;
        private Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate;

        public FuncOperationFactory(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            this.activity = activity;
            this.initialStatusMessage = initialStatusMessage;
            this.asyncMethodDelegate = asyncMethodDelegate;
        }

        public AsyncProducer<TState, TResult> CreateOperation(CancellationTokenSource tokenSource, Task<TResult> task, AsyncOpProgress<TState> progress, IObserver<AsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public string GetActivity(out string initialStatusMessage)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> InvokeAsync(AsyncOpProgress<TState> progress)
        {
            throw new NotImplementedException();
        }

        public string OnCanceled(AsyncProducer<TState, TResult> operation)
        {
            throw new NotImplementedException();
        }

        public string OnFaulted(Exception fault, AsyncProducer<TState, TResult> operation)
        {
            throw new NotImplementedException();
        }

        public string OnRanToCompletion(AsyncProducer<TState, TResult> operation)
        {
            throw new NotImplementedException();
        }
    }
}
