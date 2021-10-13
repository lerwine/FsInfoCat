using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlManagerService_Obsolete
    {
        bool IsActive { get; }

        void AddCrawlActivityEventListener([DisallowNull] ICrawlActivityEventListener listener);

        bool RemoveCrawlActivityEventListener(ICrawlActivityEventListener listener);

        void AddCrawlManagerEventListener([DisallowNull] ICrawlManagerEventListener listener);

        bool RemoveCrawlManagerEventListener(ICrawlManagerEventListener listener);

        void AddCrawlErrorEventListener([DisallowNull] ICrawlErrorEventListener listener);

        bool RemoveCrawlErrorEventListener(ICrawlErrorEventListener listener);

        void AddFileSystemItemEventListener([DisallowNull] IFileSystemItemEventListener listener);

        bool RemoveFileSystemItemEventListener(IFileSystemItemEventListener listener);

        void AddSubdirectoryCrawlEventListener([DisallowNull] ISubdirectoryCrawlEventListener listener);

        bool RemoveSubdirectoryCrawlEventListener(ISubdirectoryCrawlEventListener listener);

        void AddFileCrawlEventListener([DisallowNull] IFileCrawlEventListener listener);

        bool RemoveFileCrawlEventListener(IFileCrawlEventListener listener);

        ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt);

        ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration);

        void CancelAllCrawlsAsync();
    }
}
