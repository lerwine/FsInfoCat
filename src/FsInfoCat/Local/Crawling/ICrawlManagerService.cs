using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlMessageBus
    {
        void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener);

        bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener);

        void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlManagerEventArgs> listener);

        bool RemoveCrawlManagerEventListener(IProgress<ICrawlManagerEventArgs> listener);

        void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener);

        bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener);

        void AddFileSystemItemEventListener([DisallowNull] IProgress<ICrawlManagerFsItemEventArgs> listener, bool includeErrorEvents);

        bool RemoveFileSystemItemEventListener(IProgress<ICrawlManagerFsItemEventArgs> listener, bool includesErrorEvents);

        void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents);

        bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener, bool includesErrorEvents);

        void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents);

        bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener, bool includesErrorEvents);

        Task ReportAsync([DisallowNull] CrawlJobStartEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] CrawlJobEndEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] DirectoryCrawlStartEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] DirectoryCrawlEndEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] DirectoryCrawlErrorEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] FileCrawlStartEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] FileCrawlEndEventArgs eventArgs, CancellationToken cancellationToken);

        Task ReportAsync([DisallowNull] FileCrawlErrorEventArgs eventArgs, CancellationToken cancellationToken);
    }
    public interface ICrawlQueue
    {
        ICrawlJob ActiveJob { get; }

        Task<bool> TryEnqueueAsync(ICrawlJob crawlJob, CancellationToken cancellationToken);

        Task<bool> TryDequeueAsync(ICrawlJob crawlJob, CancellationToken cancellationToken);

        bool IsActive(ICrawlJob crawlJob);

        bool IsEnqueued(ICrawlJob crawlJob);

        bool IsEnqueuedOrActive(ICrawlJob crawlJob);

        void AddActiveStateChangedEventListener([DisallowNull] IProgress<bool> listener);

        bool RemoveActiveStateChangedEventListener(IProgress<bool> listener);

        Task CancelAllCrawlsAsync();
    }
    [Obsolete("Use ICrawlMessageBus, ICrawlQueue or ICrawlJob")]
    public interface ICrawlManagerService
    {
        bool IsActive { get; }

        void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener);

        bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener);

        void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlManagerEventArgs> listener);

        bool RemoveCrawlManagerEventListener(IProgress<ICrawlManagerEventArgs> listener);

        void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener);

        bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener);

        void AddFileSystemItemEventListener([DisallowNull] IProgress<ICrawlManagerFsItemEventArgs> listener, bool includeErrorEvents);

        bool RemoveFileSystemItemEventListener(IProgress<ICrawlManagerFsItemEventArgs> listener);

        void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents);

        bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener);

        void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents);

        bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener);

        ICrawlJob QueueCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt);

        ICrawlJob QueueCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration);

        void CancelAllCrawlsAsync();
    }
}
