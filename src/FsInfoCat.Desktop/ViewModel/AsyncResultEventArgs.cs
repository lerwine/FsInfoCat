using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AsyncResultEventArgs<T> : EventArgs, IAsyncResult
    {
        private readonly IAsyncResult _asyncResult;

        bool IAsyncResult.CompletedSynchronously => _asyncResult.CompletedSynchronously;

        public bool Success { get; }

        public T Result { get; }

        public AggregateException Exception { get; }

        bool IAsyncResult.IsCompleted => true;

        WaitHandle IAsyncResult.AsyncWaitHandle => _asyncResult.AsyncWaitHandle;

        object IAsyncResult.AsyncState => _asyncResult.AsyncState;

        public AsyncResultEventArgs(Task<T> task)
        {
            if (!task.IsCompleted)
                throw new ArgumentOutOfRangeException(nameof(task));
            _asyncResult = task;
            if (!task.IsCanceled)
            {
                if (task.IsFaulted)
                    Exception = task.Exception;
                else
                {
                    Result = task.Result;
                    Success = true;
                    return;
                }
            }
            Success = false;
        }
    }
}
