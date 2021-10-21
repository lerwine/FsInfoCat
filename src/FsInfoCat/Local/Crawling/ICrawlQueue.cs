using FsInfoCat.AsyncOps;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlQueue
    {
        ICrawlJob ActiveJob { get; }

        ICrawlJob Enqueue(ILocalCrawlConfiguration crawlConfiguration);

        ICrawlJob Enqueue(ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt);

        bool IsActive(ICrawlJob crawlJob);

        bool IsEnqueued(ICrawlJob crawlJob);

        bool IsEnqueuedOrActive(ICrawlJob crawlJob);

        void AddActiveStateChangedEventListener([DisallowNull] IProgress<bool> listener);

        bool RemoveActiveStateChangedEventListener(IProgress<bool> listener);

        Task CancelAllCrawlsAsync();
    }
}
