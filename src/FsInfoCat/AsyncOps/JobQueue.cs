using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public class JobQueue : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public IJobResult Enqueue(Func<JobContext, Task> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public IJobResult Enqueue<TJobContext>(Func<CancellationToken, Func<TimeSpan>, TJobContext> factory, Func<TJobContext, Task> doWorkAsync)
            where TJobContext : JobContext
        {
            throw new NotImplementedException();
        }

        public IJobResult Enqueue(Func<CancellationToken, Task> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public IJobResult<TResult> Enqueue<TResult>(Func<JobContext, Task<TResult>> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public IJobResult<TResult> Enqueue<TJobContext, TResult>(Func<CancellationToken, Func<TimeSpan>, TJobContext> factory, Func<TJobContext, Task<TResult>> doWorkAsync)
            where TJobContext : JobContext
        {
            throw new NotImplementedException();
        }

        public async Task EnqueueAsync(Func<JobContext, Task> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public async Task EnqueueAsync<TJobContext>(Func<CancellationToken, Func<TimeSpan>, TJobContext> factory, Func<JobContext, Task> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public async Task EnqueueAsync(Func<CancellationToken, Task> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> EnqueueAsync<TResult>(Func<JobContext, Task<TResult>> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> EnqueueAsync<TJobContext, TResult>(Func<CancellationToken, Func<TimeSpan>, TJobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> EnqueueAsync<TResult>(Func<CancellationToken, Task<TResult>> doWorkAsync)
        {
            throw new NotImplementedException();
        }
    }

    public class JobContext
    {
        private readonly IJobResult _jobResult;

        public CancellationToken CancellationToken { get; }

        public DateTime Started => _jobResult.Started;

        public TimeSpan Elapsed => _jobResult.Elapsed;

        public JobContext(CancellationToken cancellationToken, IJobResult jobResult)
        {
            _jobResult = jobResult;
            CancellationToken = cancellationToken;
        }
    }

    public interface IJobResult : IAsyncResult
    {
        DateTime Started { get; }

        TimeSpan Elapsed { get; }

        AsyncJobStatus Status { get; }

        Task GetTask();
    }

    public interface IJobResult<TResult> : IJobResult
    {
        TResult Result { get; }

        new Task<TResult> GetTask();
    }
}
