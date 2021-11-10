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

        public void Report([DisallowNull] ICrawlJobEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(CrawlJobStartEventArgs), eventArgs);
            try
            {
                if (eventArgs is CrawlJobStartEventArgs || eventArgs is CrawlJobEndEventArgs)
                    _crawlManagerEventListeners.RaiseProgressChangedAsync(eventArgs);
            }
            finally { ReportCrawlActivityEvent(eventArgs); }
        }

        public void Report([DisallowNull] DirectoryCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(DirectoryCrawlEventArgs), eventArgs);
            try
            {
                if (eventArgs is DirectoryCrawlErrorEventArgs errorEventArgs)
                    try { ReportCrawlErrorEvent(errorEventArgs); }
                    finally { _directoryEventListeners.RaiseProgressChangedAsync(eventArgs); }
                else if (eventArgs is DirectoryCrawlStartEventArgs || eventArgs is DirectoryCrawlEndEventArgs)
                    _directoryEventListeners.RaiseProgressChangedAsync(eventArgs);
            }
            finally
            {
                try { _anyDirectoryEventListeners.RaiseProgressChangedAsync(eventArgs); }
                finally { ReportFsItemEvent(eventArgs); }
            }
        }

        public void Report([DisallowNull] FileCrawlEventArgs eventArgs)
        {
            _logger.LogDebug("{Method}({Type} {eventArgs})", nameof(Report), nameof(FileCrawlEventArgs), eventArgs);
            try
            {
                if (eventArgs is FileCrawlErrorEventArgs errorEventArgs)
                    try { ReportCrawlErrorEvent(errorEventArgs); }
                    finally { _fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs); }
                else if (eventArgs is FileCrawlStartEventArgs || eventArgs is FileCrawlEndEventArgs)
                    _fileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs);
            }
            finally
            {
                try { _anyFileCrawlEventListeners.RaiseProgressChangedAsync(eventArgs); }
                finally { ReportFsItemEvent(eventArgs); }
            }
        }

        public void ReportOtherActivity(CrawlActivityEventArgs e)
        {
            if (e is ICrawlJobEventArgs eventArgs)
                Report(eventArgs);
            else
                ReportCrawlActivityEvent(e);
        }
    }
}
