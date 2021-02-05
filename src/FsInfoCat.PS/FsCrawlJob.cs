using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.PS
{
    public class FsCrawlJob : Job
    {
        public const int ACTIVITY_ID = 0;

        public const string ACTIVITY = "Crawl Subdirectory";

        private object _syncRoot = new object();
        private bool _isRunning = false;
        private Task<bool> _task;
        private string _statusMessage = "";
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Stopwatch _stopWatch;
        private readonly CancellationToken _token;
        private readonly Queue<string> _startingDirectories;
        private long _totalItemCount = 0L;
        private string _currentStartingDirectory;
        private readonly long _maxItems;
        private readonly string _machineIdentifier;

        internal Collection<FsRoot> FsRoots { get; }
        internal int MaxDepth { get; }
        public Func<IEnumerable<IVolumeInfo>> GetVolumes { get; }
        internal Func<bool> IsExpired { get; }

        public override bool HasMoreData => _isRunning || Output.Count > 0 || Error.Count > 0 || Warning.Count > 0 || Verbose.Count > 0 || Progress.Count > 0 || Debug.Count > 0 || Information.Count > 0;

        public override string Location => "";

        public override string StatusMessage => _statusMessage;

        /// <summary>
        /// Create new CrawlJob
        /// </summary>
        /// <param name="startingDirectories">Paths to be crawled.</param>
        /// <param name="maxDepth">Maximum recursion depth.</param>
        /// <param name="maxItems">Maximum number of items to crawl.</param>
        /// <param name="ttl">The number of milliseconds that the job can run or -1L for no limit.</param>
        /// <param name="friendlyName">The name of the job.</param>
        internal FsCrawlJob(IEnumerable<string> startingDirectories, int maxDepth, long maxItems, long ttl, string machineIdentifier, Func<IEnumerable<IVolumeInfo>> getVolumes, string friendlyName) : base(null, friendlyName)
        {
            MaxDepth = maxDepth;
            GetVolumes = getVolumes;
            _maxItems = maxItems;
            _token = _cancellationTokenSource.Token;
            _machineIdentifier = machineIdentifier;
            _startingDirectories = new Queue<string>((startingDirectories is null) ? new string[0] : startingDirectories.Where(p => !string.IsNullOrEmpty(p)).ToArray());
            if (ttl < 0L)
            {
                _stopWatch = null;
                IsExpired = new Func<bool>(() => _token.IsCancellationRequested);
            }
            else
            {
                _stopWatch = new Stopwatch();
                IsExpired = new Func<bool>(() =>
                {
                    if (_token.IsCancellationRequested || _stopWatch.ElapsedMilliseconds >= ttl)
                    {
                        if (_stopWatch.IsRunning)
                            _stopWatch.Stop();
                        return true;
                    }
                    return false;
                });
            }
        }

        /// <summary>
        /// Create new CrawlJob
        /// </summary>
        /// <param name="startingDirectories">Paths to be crawled.</param>
        /// <param name="maxDepth">Maximum recursion depth.</param>
        /// <param name="maxItems">Maximum number of items to crawl.</param>
        /// <param name="stopAt">When to stop the job.</param>
        /// <param name="friendlyName">The name of the job.</param>
        internal FsCrawlJob(IEnumerable<string> startingDirectories, int maxDepth, long maxItems, DateTime stopAt, string machineIdentifier, Func<IEnumerable<IVolumeInfo>> getVolumes, string friendlyName) : base(null, friendlyName)
        {
            MaxDepth = maxDepth;
            GetVolumes = getVolumes;
            _maxItems = maxItems;
            _machineIdentifier = machineIdentifier;
            _startingDirectories = new Queue<string>((startingDirectories is null) ? new string[0] : startingDirectories.Where(p => null != p).ToArray());
            _token = _cancellationTokenSource.Token;
            IsExpired = new Func<bool>(() => _token.IsCancellationRequested || DateTime.Now >= stopAt);
            _stopWatch = null;
        }

        internal void StartJob(object state)
        {
            _isRunning = true;
            _task = Task.Factory.StartNew<bool>(() =>
            {
                SetJobState(JobState.Running);
                return RunNext(state);
            }, _cancellationTokenSource.Token);
            _task.ContinueWith(CrawlCompleted, state, _cancellationTokenSource.Token);
        }

        private void CrawlCompleted(Task<bool> task, object state)
        {
            JobState jobState;
            if (task.IsCanceled)
            {
                jobState = JobState.Stopped;
                Progress.Add(new ProgressRecord(FsCrawlJob.ACTIVITY_ID, FsCrawlJob.ACTIVITY, "Aborted") { RecordType = ProgressRecordType.Completed });
            }
            else
            {
                if (task.Result)
                {
                    // if (null != result.Item2)
                    //     Output.Add(PSObject.AsPSObject(result.Item2));
                    _task = Task.Factory.StartNew<bool>(RunNext, state, _cancellationTokenSource.Token);
                    _task.ContinueWith(CrawlCompleted, _cancellationTokenSource.Token, _cancellationTokenSource.Token);
                    return;
                }
                jobState = JobState.Completed;
                Progress.Add(new ProgressRecord(FsCrawlJob.ACTIVITY_ID, FsCrawlJob.ACTIVITY, "Completed") { RecordType = ProgressRecordType.Completed });
            }
            Output.Add(PSObject.AsPSObject(new FsHost()
            {
                MachineIdentifier = _machineIdentifier,
                MachineName = Environment.MachineName,
                Roots = new Util.ComponentList<FsRoot>(FsRoots)
            }));
            _isRunning = false;
            SetJobState(jobState);
        }

        private bool RunNext(object state)
        {
            long itemsRemaining;
            if (IsExpired() || (itemsRemaining = _maxItems - _totalItemCount) < 1L)
            {
                _currentStartingDirectory = null;
                return false;
            }
            if (!_startingDirectories.TryDequeue(out string startingDirectory))
            {
                _currentStartingDirectory = null;
                return true;
            }
            _currentStartingDirectory = startingDirectory;
            bool result = CrawlWorker.Run(startingDirectory, itemsRemaining, this, out itemsRemaining);
            _totalItemCount += itemsRemaining;
            return result;
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
