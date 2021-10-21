using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public partial class JobQueue : BackgroundService
    {
        private readonly Queue<(Action Start, IPendingJob Job)> _queue = new();
        private IJobResult _current;
        private CancellationToken _stoppingToken = new(false);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            return Task.Run(() => stoppingToken.Register(OnStopping));
        }

        private void OnStopping()
        {
            List<(Action Start, IJobResult Job)> jobs = new();
            Monitor.Enter(_queue);
            try
            {
                _stoppingToken = new(true);
                Task task = _current?.GetTask();
                if (_current is not null)
                    jobs.Add((null, _current));
                while (_queue.TryDequeue(out (Action Start, IPendingJob Job) next))
                {
                    jobs.Add((next.Start, next.Job.Job));
                    next.Job.Cancel(true);
                }
            }
            finally { Monitor.Exit(_queue); }

            if (jobs.Count == 0)
                return;
            Task.WaitAll(((jobs[0].Start is null) ? jobs.Take(1).Select(j => j.Job.GetTask()).Concat(jobs.Skip(1).Select(j =>
            {
                j.Start();
                return j.Job.GetTask();
            })) : jobs.Select(j =>
            {
                j.Start();
                return j.Job.GetTask();
            })).ToArray());
        }

        private void SetCurrent(IJobResult job)
        {
            _current = job;
            Task.Run(() =>
            {
                job.AsyncWaitHandle.WaitOne();
                Monitor.Enter(_queue);
                try
                {
                    if (_queue.TryDequeue(out (Action Start, IPendingJob Job) next))
                    {
                        SetCurrent(next.Job.Job);
                        next.Start();
                    }
                }
                finally { Monitor.Exit(_queue); }
            });
        }

        private IJobResult OnEnqueue(Func<IJobResult, CancellationToken, JobContext> factory, Func<JobContext, Task> doWorkAsync)
        {
            Monitor.Enter(_queue);
            try
            {
                _stoppingToken.ThrowIfCancellationRequested();
                if (_current is null)
                {
                    IJobResult jobResult = new JobResult(factory, doWorkAsync);
                    SetCurrent(jobResult);
                    return jobResult;
                }
                (Action Start, IPendingJob Job) pending = JobResult.Create((jr, ct) => new JobContext(jr, ct), doWorkAsync);
                _queue.Enqueue(pending);
                return pending.Job;
            }
            finally { Monitor.Exit(_queue); }
        }

        private IJobResult<TResult> OnEnqueue<TResult>(Func<IJobResult<TResult>, CancellationToken, JobContext> factory, Func<JobContext, Task<TResult>> doWorkAsync)
        {
            Monitor.Enter(_queue);
            try
            {
                _stoppingToken.ThrowIfCancellationRequested();
                if (_current is null)
                {
                    IJobResult<TResult> jobResult = new JobResult<TResult>(factory, doWorkAsync);
                    SetCurrent(jobResult);
                    return jobResult;
                }
                (Action Start, IPendingJob<TResult> Job) pending = JobResult<TResult>.Create((jr, ct) => new JobContext(jr, ct), doWorkAsync);
                _queue.Enqueue(pending);
                return pending.Job;
            }
            finally { Monitor.Exit(_queue); }
        }

        public IJobResult Enqueue(Func<JobContext, Task> doWorkAsync) => OnEnqueue((jr, ct) => new JobContext(jr, ct), doWorkAsync);

        public IJobResult Enqueue(Func<CancellationToken, Task> doWorkAsync) => OnEnqueue((jr, ct) => new JobContext(jr, ct), jc => doWorkAsync(jc.CancellationToken));

        public IJobResult<TResult> Enqueue<TResult>(Func<JobContext, Task<TResult>> doWorkAsync) => OnEnqueue((jr, ct) => new JobContext(jr, ct), doWorkAsync);

        public IJobResult<TResult> Enqueue<TResult>(Func<CancellationToken, Task<TResult>> doWorkAsync) => OnEnqueue((jr, ct) => new JobContext(jr, ct), jc => doWorkAsync(jc.CancellationToken));

        public async Task EnqueueAsync(Func<JobContext, Task> doWorkAsync) => await Enqueue(doWorkAsync).GetTask();

        public async Task EnqueueAsync(Func<CancellationToken, Task> doWorkAsync) => await Enqueue(doWorkAsync).GetTask();

        public async Task<TResult> EnqueueAsync<TResult>(Func<JobContext, Task<TResult>> doWorkAsync) => await Enqueue(doWorkAsync).GetTask();

        public async Task<TResult> EnqueueAsync<TResult>(Func<CancellationToken, Task<TResult>> doWorkAsync) => await Enqueue(doWorkAsync).GetTask();
    }
}
