using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlMessageBus : ICrawlProgress
    {
        void AddAsyncEventListener([DisallowNull] IProgress<IAsyncOperationInfo> listener);

        bool RemoveAsyncEventListener(IProgress<IAsyncOperationInfo> listener);

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
    }
}
