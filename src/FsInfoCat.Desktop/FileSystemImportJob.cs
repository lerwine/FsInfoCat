using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Represents a background task for crawling the contents of a subdirectory.
    /// </summary>
    [Obsolete("Use FsInfoCat.Local.CrawlWorker, instead")]
    public partial class FileSystemImportJob : IDisposable
    {
        private readonly object _syncRoot = new();
        private CancellationTokenSource _tokenSource = new();

        /// <summary>
        /// Maximum number of subdirectories to crawl.
        /// </summary>
        /// <remarks>The value of this property is set according to the value of the <see cref="CrawlConfiguration.MaxTotalItems">MaxTotalItems</see>
        /// of the <see cref="CrawlConfiguration" /> that was passed to the
        /// <see cref="FileSystemImportJob(CrawlConfiguration, FileSystemImportObserver)">constructor</see>.
        /// </remarks>
        public ushort MaxRecursionDepth { get; }

        /// <summary>
        /// Maximum number of items (file and subdirectories) to process on the host filesystem.
        /// </summary>
        /// <remarks>The value of this property is set according to the value of the <see cref="CrawlConfiguration.MaxTotalItems">MaxTotalItems</see>
        /// of the <see cref="CrawlConfiguration" /> that was passed to the
        /// <see cref="FileSystemImportJob(CrawlConfiguration, FileSystemImportObserver)">constructor</see>.
        /// </remarks>
        public ulong MaxTotalItems { get; }

        /// <summary>
        /// Gets the <see cref="CrawlConfiguration.DisplayName">display name</see> of the <see cref="CrawlConfiguration" /> that was passed to the
        /// <see cref="FileSystemImportJob(CrawlConfiguration, FileSystemImportObserver)">constructor</see>.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the <see cref="Task">background task</see> for the crawl job.
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemImportJob"/> class, starting the crawl job in a background task.
        /// </summary>
        /// <param name="configuration">The crawl job configuration.</param>
        /// <param name="observer">The optional object that receives crawl job events.</param>
        public FileSystemImportJob([NotNull] CrawlConfiguration configuration, FileSystemImportObserver observer = null)
        {
            MaxRecursionDepth = (configuration ?? throw new ArgumentNullException(nameof(configuration))).MaxRecursionDepth;
            MaxTotalItems = configuration.MaxTotalItems;
            DisplayName = configuration.DisplayName;
            Task = ScanContext.RunAsync(configuration, observer, _tokenSource.Token);
            Task.ContinueWith(task =>
            {
                try
                {
                    if (task.IsCanceled)
                        observer?.RaiseJobCanceled();
                    else if (task.IsFaulted)
                        observer?.RaiseJobFailed(task.Exception);
                    else
                        observer?.RaiseJobCompleted();
                }
                finally
                {
                    CancellationTokenSource tokenSource;
                    lock (_syncRoot)
                    {
                        tokenSource = _tokenSource;
                        _tokenSource = null;
                    }
                    tokenSource?.Dispose();
                }
            });
        }

        public void Dispose()
        {
            CancellationTokenSource tokenSource;
            lock (_syncRoot)
            {
                tokenSource = _tokenSource;
                _tokenSource = null;
            }
            if (tokenSource is null)
                return;
            using (tokenSource)
            {
                if (!Task.IsCompleted)
                {
                    tokenSource.Cancel(true);
                    Thread.Sleep(0);
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
