using System;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public class JobProgressEventArgs : EventArgs
    {
        public string Message { get; init; }
        public StatusMessageLevel Level { get; init; }
        public string Detail { get; init; }
        public Exception Exception { get; init; }
        public bool IsCompleted { get; init; }
    }
    public class JobProgressEventArgs<TState> : JobProgressEventArgs
    {
        public TState State { get; init; }
    }
}
