using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
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
