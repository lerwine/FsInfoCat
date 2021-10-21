using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public partial class JobQueue
    {
        class JobResult : ICancellableJob
        {
            private readonly Task _task;
            private readonly Stopwatch _stopwatch = new();
            private CancellationTokenSource _tokenSource = new();
            private readonly WaitHandleRelay _waitHandle = new();

            public DateTime Started { get; private set; }

            public TimeSpan Elapsed => _stopwatch.Elapsed;

            public AsyncJobStatus Status { get; private set; }

            public object AsyncState => ((IAsyncResult)_task).AsyncState;

            public WaitHandle AsyncWaitHandle => _waitHandle;

            public bool CompletedSynchronously => IsCompleted && ((IAsyncResult)_task).CompletedSynchronously;

            public bool IsCompleted { get; private set; }

            public bool IsCancellationRequested => _tokenSource?.IsCancellationRequested ?? Status == AsyncJobStatus.Canceled;

            public Task GetTask() => _task;

            private async Task RunAsync(Func<IJobResult, CancellationToken, JobContext> factory, Func<JobContext, Task> doWorkAsync, WaitHandle waitHandle)
            {
                waitHandle?.WaitOne();
                Status = AsyncJobStatus.Running;
                _stopwatch.Start();
                Started = DateTime.Now;
                try { await doWorkAsync(factory(this, _tokenSource.Token)); }
                finally { _stopwatch.Stop(); }
            }

            private JobResult(Func<IJobResult, CancellationToken, JobContext> factory, Func<JobContext, Task> doWorkAsync, WaitHandle waitHandle)
            {
                _task = RunAsync(factory, doWorkAsync, waitHandle);
                _task.ContinueWith(t =>
                {
                    try
                    {
                        try
                        {
                            CancellationTokenSource tokenSource = _tokenSource;
                            _tokenSource = null;
                            tokenSource.Dispose();
                        }
                        finally { Status = t.IsCanceled ? AsyncJobStatus.Canceled : t.IsFaulted ? AsyncJobStatus.Faulted : AsyncJobStatus.Succeeded; }
                    }
                    finally
                    {
                        try { IsCompleted = true; }
                        finally { _waitHandle.BackingHandle.Set(); }
                    }
                });
            }

            internal JobResult(Func<IJobResult, CancellationToken, JobContext> factory, Func<JobContext, Task> doWorkAsync) : this(factory, doWorkAsync, null) { }

            internal static (Action Start, IPendingJob Job) Create(Func<IJobResult, CancellationToken, JobContext> factory, Func<JobContext, Task> doWorkAsync)
            {
                AutoResetEvent waitHandle = new(false);
                JobResult jobResult = new(factory, doWorkAsync, waitHandle);
                jobResult.GetTask().ContinueWith(t => waitHandle.Dispose());
                return (() => waitHandle.Reset(), new PendingJob(jobResult));
            }

            public void Cancel()
            {
                switch (Status)
                {
                    case AsyncJobStatus.Running:
                    case AsyncJobStatus.WaitingToRun:
                        Status = AsyncJobStatus.Cancelling;
                        break;
                }
                _tokenSource?.Cancel();
            }

            public void Cancel(bool throwOnFirstException)
            {
                switch (Status)
                {
                    case AsyncJobStatus.Running:
                    case AsyncJobStatus.WaitingToRun:
                        Status = AsyncJobStatus.Cancelling;
                        break;
                }
                _tokenSource?.Cancel(throwOnFirstException);
            }

            class PendingJob : IPendingJob
            {
                private readonly JobResult _job;

                internal PendingJob(JobResult job) => _job = job;

                public IJobResult Job => _job;

                public bool IsCancellationRequested => _job.IsCancellationRequested;

                DateTime IJobResult.Started => _job.Started;

                TimeSpan IJobResult.Elapsed => _job.Elapsed;

                AsyncJobStatus IJobResult.Status => _job.Status;

                object IAsyncResult.AsyncState => _job.Started;

                WaitHandle IAsyncResult.AsyncWaitHandle => _job.AsyncWaitHandle;

                bool IAsyncResult.CompletedSynchronously => _job.CompletedSynchronously;

                bool IAsyncResult.IsCompleted => _job.IsCompleted;

                public void Cancel() => _job.Cancel();

                public void Cancel(bool throwOnFirstException) => _job.Cancel(throwOnFirstException);

                public void CancelAfter(int millisecondsDelay) => _job._tokenSource?.CancelAfter(millisecondsDelay);

                public void CancelAfter(TimeSpan delay) => _job._tokenSource?.CancelAfter(delay);

                Task IJobResult.GetTask() => _job.GetTask();
            }

            class WaitHandleRelay : WaitHandle
            {
                internal ManualResetEvent BackingHandle { get; } = new(false);

                [Obsolete]
                public override IntPtr Handle { get => BackingHandle.Handle; set => BackingHandle.Handle = value; }

                public override bool WaitOne() => BackingHandle.WaitOne();

                public override bool WaitOne(int millisecondsTimeout) => BackingHandle.WaitOne(millisecondsTimeout);

                public override bool WaitOne(int millisecondsTimeout, bool exitContext) => BackingHandle.WaitOne(millisecondsTimeout, exitContext);

                public override bool WaitOne(TimeSpan timeout) => BackingHandle.WaitOne(timeout);

                public override bool WaitOne(TimeSpan timeout, bool exitContext) => BackingHandle.WaitOne(timeout, exitContext);

                public override void Close() => BackingHandle.Close();

                protected override void Dispose(bool explicitDisposing)
                {
                    if (explicitDisposing)
                        BackingHandle.Dispose();
                    base.Dispose(explicitDisposing);
                }
            }
        }

        class JobResult<TResult> : IJobResult<TResult>, ICancellableJob
        {
            private readonly Task<TResult> _task;
            private readonly Stopwatch _stopwatch = new();
            private CancellationTokenSource _tokenSource = new();
            private readonly WaitHandleRelay _waitHandle = new();

            public TResult Result
            {
                get
                {
                    _waitHandle.WaitOne();
                    return _task.Result;
                }
            }

            public DateTime Started { get; private set; }

            public TimeSpan Elapsed => _stopwatch.Elapsed;

            public AsyncJobStatus Status { get; private set; }

            public object AsyncState => ((IAsyncResult)_task).AsyncState;

            public WaitHandle AsyncWaitHandle => _waitHandle;

            public bool CompletedSynchronously => IsCompleted && ((IAsyncResult)_task).CompletedSynchronously;

            public bool IsCompleted { get; private set; }

            public bool IsCancellationRequested => _tokenSource?.IsCancellationRequested ?? Status == AsyncJobStatus.Canceled;

            public void Cancel()
            {
                switch (Status)
                {
                    case AsyncJobStatus.Running:
                    case AsyncJobStatus.WaitingToRun:
                        Status = AsyncJobStatus.Cancelling;
                        break;
                }
                _tokenSource?.Cancel();
            }

            public void Cancel(bool throwOnFirstException)
            {
                switch (Status)
                {
                    case AsyncJobStatus.Running:
                    case AsyncJobStatus.WaitingToRun:
                        Status = AsyncJobStatus.Cancelling;
                        break;
                }
                _tokenSource?.Cancel(throwOnFirstException);
            }

            private async Task<TResult> RunAsync(Func<IJobResult<TResult>, CancellationToken, JobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync, WaitHandle waitHandle)
            {
                waitHandle?.WaitOne();
                Status = AsyncJobStatus.Running;
                _stopwatch.Start();
                Started = DateTime.Now;
                try { return await doWorkAsync(factory(this, _tokenSource.Token)); }
                finally { _stopwatch.Stop(); }
            }

            private JobResult(Func<IJobResult<TResult>, CancellationToken, JobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync, WaitHandle waitHandle)
            {
                _task = RunAsync(factory, doWorkAsync, waitHandle);
                _task.ContinueWith(t =>
                {
                    try
                    {
                        try
                        {
                            CancellationTokenSource tokenSource = _tokenSource;
                            _tokenSource = null;
                            tokenSource.Dispose();
                        }
                        finally { Status = t.IsCanceled ? AsyncJobStatus.Canceled : t.IsFaulted ? AsyncJobStatus.Faulted : AsyncJobStatus.Succeeded; }
                    }
                    finally
                    {
                        try { IsCompleted = true; }
                        finally { _waitHandle.BackingHandle.Set(); }
                    }
                });
            }

            public Task<TResult> GetTask() => _task;

            Task IJobResult.GetTask() => GetTask();

            internal JobResult(Func<IJobResult<TResult>, CancellationToken, JobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync) : this(factory, doWorkAsync, null) { }

            internal static (Action Start, IPendingJob<TResult> Job) Create(Func<IJobResult<TResult>, CancellationToken, JobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync)
            {
                AutoResetEvent waitHandle = new(false);
                JobResult<TResult> jobResult = new(factory, doWorkAsync, waitHandle);
                jobResult.GetTask().ContinueWith(t => waitHandle.Dispose());
                return (() => waitHandle.Reset(), new PendingJob(jobResult));
            }

            class PendingJob : IPendingJob<TResult>
            {
                private readonly JobResult<TResult> _job;

                internal PendingJob(JobResult<TResult> job) => _job = job;

                public IJobResult<TResult> Job => _job;

                public bool IsCancellationRequested => _job.IsCancellationRequested;

                IJobResult IPendingJob.Job => Job;

                TResult IJobResult<TResult>.Result => _job.Result;

                DateTime IJobResult.Started => _job.Started;

                TimeSpan IJobResult.Elapsed => _job.Elapsed;

                AsyncJobStatus IJobResult.Status => _job.Status;

                object IAsyncResult.AsyncState => _job.AsyncState;

                WaitHandle IAsyncResult.AsyncWaitHandle => _job.AsyncWaitHandle;

                bool IAsyncResult.CompletedSynchronously => _job.CompletedSynchronously;

                bool IAsyncResult.IsCompleted => _job.IsCompleted;

                public void Cancel() => _job.Cancel();

                public void Cancel(bool throwOnFirstException) => _job.Cancel(throwOnFirstException);

                public void CancelAfter(int millisecondsDelay) => _job._tokenSource?.CancelAfter(millisecondsDelay);

                public void CancelAfter(TimeSpan delay) => _job._tokenSource?.CancelAfter(delay);

                Task<TResult> IJobResult<TResult>.GetTask() => _job.GetTask();

                Task IJobResult.GetTask() => _job.GetTask();
            }

            class WaitHandleRelay : WaitHandle
            {
                internal ManualResetEvent BackingHandle { get; } = new(false);

                [Obsolete]
                public override IntPtr Handle { get => BackingHandle.Handle; set => BackingHandle.Handle = value; }

                public override bool WaitOne() => BackingHandle.WaitOne();

                public override bool WaitOne(int millisecondsTimeout) => BackingHandle.WaitOne(millisecondsTimeout);

                public override bool WaitOne(int millisecondsTimeout, bool exitContext) => BackingHandle.WaitOne(millisecondsTimeout, exitContext);

                public override bool WaitOne(TimeSpan timeout) => BackingHandle.WaitOne(timeout);

                public override bool WaitOne(TimeSpan timeout, bool exitContext) => BackingHandle.WaitOne(timeout, exitContext);

                public override void Close() => BackingHandle.Close();

                protected override void Dispose(bool explicitDisposing)
                {
                    if (explicitDisposing)
                        BackingHandle.Dispose();
                    base.Dispose(explicitDisposing);
                }
            }
        }
    }
}
