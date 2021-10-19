using FsInfoCat.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public class CrawlMessageBus : ICrawlMessageBus
    {
        private readonly ILogger<CrawlMessageBus> _logger;
        private readonly WeakReferenceSet<IProgress<ICrawlActivityEventArgs>> _crawlActivityEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlErrorEventArgs>> _crawlErrorEventListeners = new();
        private readonly WeakReferenceSet<IProgress<ICrawlJobEventArgs>> _crawlManagerEventListeners = new();
        private readonly WeakReferenceSet<IProgress<FileCrawlEventArgs>> _anyFileCrawlEventListeners = new();
        private readonly WeakReferenceSet<IProgress<FileCrawlEventArgs>> _fileCrawlEventListeners = new();
        private readonly WeakReferenceSet<IProgress<IFsItemCrawlEventArgs>> _anyFsItemEventListeners = new();
        private readonly WeakReferenceSet<IProgress<IFsItemCrawlEventArgs>> _fsItemEventListeners = new();
        private readonly WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _anyDirectoryEventListeners = new();
        private readonly WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _directoryEventListeners = new();

        [ServiceBuilderHandler]
        public static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(CrawlMessageBus).FullName}.{nameof(ConfigureServices)}");
            services.AddSingleton<ICrawlMessageBus, CrawlMessageBus>();
        }

        public CrawlMessageBus(ILogger<CrawlMessageBus> logger)
        {
            _logger = logger;
            _logger.LogDebug($"{nameof(ICrawlMessageBus)} Service instantiated");
        }

        public void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlJobEventArgs> listener) => _crawlManagerEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileSystemItemEventListener([DisallowNull] IProgress<IFsItemCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Remove(listener);

        public bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Remove(listener);

        public bool RemoveCrawlManagerEventListener(IProgress<ICrawlJobEventArgs> listener) => _crawlManagerEventListeners.Remove(listener);

        public bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners).Remove(listener);

        public bool RemoveFileSystemItemEventListener(IProgress<IFsItemCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners).Remove(listener);

        public bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners).Remove(listener);

        private async Task ReportCrawlActivityEventAsync([DisallowNull] ICrawlActivityEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({eventArgs}, {cancellationToken})", nameof(ReportCrawlActivityEventAsync), eventArgs, cancellationToken);
            await _crawlActivityEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken);
        }

        private async Task ReportCrawlErrorEventAsync([DisallowNull] ICrawlErrorEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({eventArgs}, {cancellationToken})", nameof(ReportCrawlErrorEventAsync), eventArgs, cancellationToken);
            await Task.WhenAll(_crawlErrorEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportCrawlActivityEventAsync(eventArgs, cancellationToken));
        }

        private async Task ReportAnyFsItemEventAsync([DisallowNull] IFsItemCrawlEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({eventArgs}, {cancellationToken})", nameof(ReportAnyFsItemEventAsync), eventArgs, cancellationToken);
            await Task.WhenAll(_anyFsItemEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportCrawlActivityEventAsync(eventArgs, cancellationToken));
        }

        private async Task ReportFsItemEventAsync([DisallowNull] IFsItemCrawlEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({eventArgs}, {cancellationToken})", nameof(ReportFsItemEventAsync), eventArgs, cancellationToken);
            await Task.WhenAll(_fsItemEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportAnyFsItemEventAsync(eventArgs, cancellationToken));
        }

        private async Task ReportAnyAsync([DisallowNull] DirectoryCrawlEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAnyAsync), nameof(DirectoryCrawlEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_anyDirectoryEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportFsItemEventAsync(eventArgs, cancellationToken));
        }

        private async Task ReportAnyAsync([DisallowNull] FileCrawlEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAnyAsync), nameof(FileCrawlEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_anyFileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportFsItemEventAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] CrawlJobStartEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(CrawlJobStartEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_crawlManagerEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportCrawlActivityEventAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] CrawlJobEndEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(CrawlJobEndEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_crawlManagerEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportCrawlActivityEventAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] DirectoryCrawlStartEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(DirectoryCrawlStartEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_directoryEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] DirectoryCrawlEndEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(DirectoryCrawlEndEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_directoryEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] DirectoryCrawlErrorEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(DirectoryCrawlErrorEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(ReportCrawlErrorEventAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] FileCrawlStartEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(FileCrawlStartEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] FileCrawlEndEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(FileCrawlEndEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(_fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }

        public async Task ReportAsync([DisallowNull] FileCrawlErrorEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs}, {cancellationToken})", nameof(ReportAsync), nameof(FileCrawlErrorEventArgs), eventArgs, cancellationToken);
            await Task.WhenAll(ReportCrawlErrorEventAsync(eventArgs, cancellationToken), ReportAnyAsync(eventArgs, cancellationToken));
        }
    }
}
