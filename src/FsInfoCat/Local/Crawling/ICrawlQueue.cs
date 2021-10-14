using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
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
}
