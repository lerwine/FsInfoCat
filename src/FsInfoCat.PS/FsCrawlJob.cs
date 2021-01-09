using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using FsInfoCat.Models.Crawl;

namespace FsInfoCat.PS
{
    public class FsCrawlJob : Job
    {
        public const int ACTIVITY_ID = 0;
        public const string ACTIVITY = "Crawl Subdirectory";
        private object _syncRoot = new object();
        private bool _isRunning = false;
        private string _statusMessage;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task<Tuple<bool, FsHost>> _task;
        private readonly CrawlWorker _worker;

        public override string StatusMessage => _statusMessage;

        public override bool HasMoreData => _isRunning || Output.Count > 0 || Error.Count > 0 || Warning.Count > 0 || Verbose.Count > 0 || Progress.Count > 0 || Debug.Count > 0 || Information.Count > 0;

        public override string Location => "";

        public bool RanToCompletion => _worker.RanToCompletion;

        internal FsCrawlJob(string friendlyName, int maxDepth, long maxItems, long ttl, Collection<string> rootPath) : base(null, friendlyName)
        {
            _worker = new CrawlWorker(_cancellationTokenSource.Token, maxDepth, maxItems, ttl, rootPath, this);
        }

        internal FsCrawlJob(string friendlyName, int maxDepth, long maxItems, DateTime stopAt, Collection<string> rootPath) : base(null, friendlyName)
        {
            _worker = new CrawlWorker(_cancellationTokenSource.Token, maxDepth, maxItems, stopAt, rootPath, this);
        }

        internal void StartJob(object state)
        {
            _isRunning = true;
            _task = Task.Factory.StartNew<Tuple<bool, FsHost>>(() =>
            {
                SetJobState(JobState.Running);
                return RunNext(state);
            }, _cancellationTokenSource.Token);
            _task.ContinueWith(CrawlCompleted, state, _cancellationTokenSource.Token);
        }

        private void CrawlCompleted(Task<Tuple<bool, FsHost>> task, object state)
        {
            JobState jobState;
            if (task.IsCanceled)
                jobState = JobState.Stopped;
            else
            {
                Tuple<bool, FsHost> result = task.Result;
                if (result.Item1)
                {
                    if (null != result.Item2)
                        Output.Add(PSObject.AsPSObject(result.Item2));
                    _task = Task.Factory.StartNew<Tuple<bool, FsHost>>(RunNext, state, _cancellationTokenSource.Token);
                    _task.ContinueWith(CrawlCompleted, _cancellationTokenSource.Token, _cancellationTokenSource.Token);
                    return;
                }
                jobState = JobState.Completed;
            }
            _isRunning = false;
            Progress.Add(new ProgressRecord(FsCrawlJob.ACTIVITY_ID, FsCrawlJob.ACTIVITY, (_worker.RanToCompletion) ? "Completed" : "Aborted") { RecordType = ProgressRecordType.Completed });
            SetJobState(jobState);
        }

        private Tuple<bool, FsHost> RunNext(object state)
        {
            return new Tuple<bool, FsHost>(_worker.ProcessNext(out FsHost result), result);
        }

        public override void StopJob()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                SetJobState(JobState.Stopping);
                _cancellationTokenSource.Cancel(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                    _cancellationTokenSource.Cancel(true);
                _cancellationTokenSource.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
