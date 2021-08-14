using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public partial class BackgroundJobViewModel
    {
        public class Job<TTask, TState> : IStateJob<TState, TTask> where TTask : Task
        {
            private bool _disposed;
            private readonly CancellationTokenSource _tokenSource;

            public event EventHandler<JobProgressEventArgs<TState>> ProgressChanged;
            private event EventHandler<JobProgressEventArgs> JobProgressChanged;
            event EventHandler<JobProgressEventArgs> IJob.ProgressChanged
            {
                add => JobProgressChanged += value;
                remove => JobProgressChanged -= value;
            }

            public TTask Task { get; private set; }

            Task IJob.Task => Task;

            private Job(CancellationTokenSource tokenSource)
            {
                _tokenSource = tokenSource;
            }

            private void RaiseProgressChanged(JobProgressEventArgs<TState> args)
            {
                try { ProgressChanged?.Invoke(this, args); }
                finally { JobProgressChanged?.Invoke(this, args); }
            }

            internal static Job<TTask, TState> Create(TState state, BackgroundJobViewModel viewModel, Func<Context<TState>, TTask> factory, Action<JobProgressEventArgs<TState>> onProgressChanged)
            {
                CancellationTokenSource tokenSource = new();
                Job<TTask, TState> job = new(tokenSource);
                Context<TState> context;
                if (onProgressChanged is null)
                    context = new(state, tokenSource.Token, viewModel, job.RaiseProgressChanged);
                else
                    context = new(state, tokenSource.Token, viewModel, e =>
                    {
                        try { onProgressChanged(e); }
                        finally { job.RaiseProgressChanged(e); }
                    });
                job.Task = factory(context);
                return job;
            }

            internal static Job<TTask, TState> Create(TState state, BackgroundJobViewModel viewModel, Func<Context<TState>, TTask> factory, Action<JobProgressEventArgs> onProgressChanged)
            {
                CancellationTokenSource tokenSource = new();
                Job<TTask, TState> job = new(tokenSource);
                Context<TState> context;
                if (onProgressChanged is null)
                    context = new(state, tokenSource.Token, viewModel, job.RaiseProgressChanged);
                else
                    context = new(state, tokenSource.Token, viewModel, e =>
                    {
                        try { onProgressChanged(e); }
                        finally { job.RaiseProgressChanged(e); }
                    });
                job.Task = factory(context);
                return job;
            }

            public void Cancel(bool throwOnFirstException)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(Job<TTask, TState>));
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel(throwOnFirstException);
            }

            public void Cancel()
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(Job<TTask, TState>));
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    _disposed = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~Job()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
