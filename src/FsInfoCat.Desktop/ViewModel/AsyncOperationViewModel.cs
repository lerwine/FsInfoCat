using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AsyncOperationViewModel<TResult> : DependencyObject, IAsyncResult, IAsyncDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;

        bool IAsyncResult.IsCompleted => throw new NotImplementedException();

        WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();

        object IAsyncResult.AsyncState => throw new NotImplementedException();

        bool IAsyncResult.CompletedSynchronously => throw new NotImplementedException();

        public void Start(IValueTaskSource<TResult> source)
        {
            source.
        }
    }
}
