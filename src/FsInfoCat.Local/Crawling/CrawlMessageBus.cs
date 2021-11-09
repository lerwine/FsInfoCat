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
        private readonly WeakReferenceSet<IProgress<IAsyncOperationInfo>> _asyncEventListeners = new();
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

        public void AddAsyncEventListener([DisallowNull] IProgress<IAsyncOperationInfo> listener) => _asyncEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlJobEventArgs> listener) => _crawlManagerEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileSystemItemEventListener([DisallowNull] IProgress<IFsItemCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public bool RemoveAsyncEventListener(IProgress<IAsyncOperationInfo> listener) => _asyncEventListeners.Remove(listener);

        public bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Remove(listener);

        public bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Remove(listener);

        public bool RemoveCrawlManagerEventListener(IProgress<ICrawlJobEventArgs> listener) => _crawlManagerEventListeners.Remove(listener);

        public bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners).Remove(listener);

        public bool RemoveFileSystemItemEventListener(IProgress<IFsItemCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners).Remove(listener);

        public bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener, bool includesErrorEvents) => (includesErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners).Remove(listener);

        private void ReportCrawlActivityEvent([DisallowNull] ICrawlActivityEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({eventArgs})", nameof(ReportCrawlActivityEvent), eventArgs);
            try { _crawlActivityEventListeners.RaiseProgressChangedAsync(eventArgs); }
            finally { _asyncEventListeners.RaiseProgressChangedAsync(eventArgs); }
        }

        private void ReportCrawlErrorEvent([DisallowNull] ICrawlErrorEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({eventArgs})", nameof(ReportCrawlErrorEvent), eventArgs);
            _crawlErrorEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportCrawlActivityEvent(eventArgs);
        }

        private void ReportAnyFsItemEvent([DisallowNull] IFsItemCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({eventArgs})", nameof(ReportAnyFsItemEvent), eventArgs);
            _anyFsItemEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportCrawlActivityEvent(eventArgs);
        }

        private void ReportFsItemEvent([DisallowNull] IFsItemCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({eventArgs})", nameof(ReportFsItemEvent), eventArgs);
            _fsItemEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportAnyFsItemEvent(eventArgs);
        }

        private void ReportAny([DisallowNull] DirectoryCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(ReportAny), nameof(DirectoryCrawlEventArgs), eventArgs);
            _anyDirectoryEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportFsItemEvent(eventArgs);
        }

        private void ReportAny([DisallowNull] FileCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(ReportAny), nameof(FileCrawlEventArgs), eventArgs);
            _anyFileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportFsItemEvent(eventArgs);
        }

        public void Report([DisallowNull] CrawlJobStartEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(CrawlJobStartEventArgs), eventArgs);
            _crawlManagerEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportCrawlActivityEvent(eventArgs);
        }

        public void Report([DisallowNull] CrawlJobEndEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(CrawlJobEndEventArgs), eventArgs);
            _crawlManagerEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportCrawlActivityEvent(eventArgs);
        }

        public void Report([DisallowNull] DirectoryCrawlStartEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(DirectoryCrawlStartEventArgs), eventArgs);
            _directoryEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportAny(eventArgs);
        }

        public void Report([DisallowNull] DirectoryCrawlEndEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(DirectoryCrawlEndEventArgs), eventArgs);
            _directoryEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportAny(eventArgs);
        }

        public void Report([DisallowNull] DirectoryCrawlErrorEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(DirectoryCrawlErrorEventArgs), eventArgs);
            ReportCrawlErrorEvent(eventArgs);
            ReportAny(eventArgs);
        }

        public void Report([DisallowNull] FileCrawlStartEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(FileCrawlStartEventArgs), eventArgs);
            _fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportAny(eventArgs);
        }

        public void Report([DisallowNull] FileCrawlEndEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(FileCrawlEndEventArgs), eventArgs);
            _fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs);
            ReportAny(eventArgs);
        }

        public void Report([DisallowNull] FileCrawlErrorEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(FileCrawlErrorEventArgs), eventArgs);
            ReportCrawlErrorEvent(eventArgs);
            ReportAny(eventArgs);
        }
    }
}
