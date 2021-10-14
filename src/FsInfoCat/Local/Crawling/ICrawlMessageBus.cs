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

        void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlJobEventArgs> listener);

        bool RemoveCrawlManagerEventListener(IProgress<ICrawlJobEventArgs> listener);

        void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener);

        bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener);

        void AddFileSystemItemEventListener([DisallowNull] IProgress<IFsItemCrawlEventArgs> listener, bool includeErrorEvents);

        bool RemoveFileSystemItemEventListener(IProgress<IFsItemCrawlEventArgs> listener, bool includesErrorEvents);

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
}
