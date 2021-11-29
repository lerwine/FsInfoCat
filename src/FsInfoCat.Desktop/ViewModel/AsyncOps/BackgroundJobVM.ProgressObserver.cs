using FsInfoCat.AsyncOps;
using System;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        internal class ProgressObserver : IObserver<IBackgroundProgressEvent>
        {
            private BackgroundJobVM _target;
            private Action<BackgroundJobVM> _onCompleted;

            public ProgressObserver(BackgroundJobVM backgroundJobVM, Action<BackgroundJobVM> onCompleted)
            {
                _target = backgroundJobVM;
                _onCompleted = onCompleted;
            }

            public void OnCompleted() => _target.Dispatcher.Invoke(() =>
            {
                try { _target.Cancel.IsEnabled = false; }
                finally { _onCompleted(_target); }
            });

            public void OnError(Exception error) => _target.Dispatcher.InvokeAsync(() => _target.OnError(error));

            public void OnNext(IBackgroundProgressEvent value) => _target.Dispatcher.InvokeAsync(() => _target.OnProgressEvent(value));
        }
    }
}
