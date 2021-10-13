using FsInfoCat.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace FsInfoCat.Local.Crawling
{
    internal partial class CrawlManagerService : ICrawlManagerService
    {
        public bool IsActive => throw new NotImplementedException();

        private WeakReferenceSet<IProgress<ICrawlActivityEventArgs>> _crawlActivityEventListeners = new();
        private WeakReferenceSet<IProgress<ICrawlErrorEventArgs>> _crawlErrorEventListeners = new();
        private WeakReferenceSet<IProgress<ICrawlManagerEventArgs>> _crawlManagerEventListeners = new();
        private WeakReferenceSet<IProgress<FileCrawlEventArgs>> _anyFileCrawlEventListeners = new();
        private WeakReferenceSet<IProgress<FileCrawlEventArgs>> _fileCrawlEventListeners = new();
        private WeakReferenceSet<IProgress<ICrawlManagerFsItemEventArgs>> _anyFsItemEventListeners = new();
        private WeakReferenceSet<IProgress<ICrawlManagerFsItemEventArgs>> _fsItemEventListeners = new();
        private WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _anyDirectoryEventListeners = new();
        private WeakReferenceSet<IProgress<DirectoryCrawlEventArgs>> _directoryEventListeners = new();

        public void AddCrawlActivityEventListener([DisallowNull] IProgress<ICrawlActivityEventArgs> listener) => _crawlActivityEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlErrorEventListener([DisallowNull] IProgress<ICrawlErrorEventArgs> listener) => _crawlErrorEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddCrawlManagerEventListener([DisallowNull] IProgress<ICrawlManagerEventArgs> listener) => _crawlManagerEventListeners.Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileCrawlEventListener([DisallowNull] IProgress<FileCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFileCrawlEventListeners : _fileCrawlEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddFileSystemItemEventListener([DisallowNull] IProgress<ICrawlManagerFsItemEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyFsItemEventListeners : _fsItemEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void AddSubdirectoryCrawlEventListener([DisallowNull] IProgress<DirectoryCrawlEventArgs> listener, bool includeErrorEvents) => (includeErrorEvents ? _anyDirectoryEventListeners : _directoryEventListeners)
            .Add(listener ?? throw new ArgumentNullException(nameof(listener)));

        public void CancelAllCrawlsAsync()
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlActivityEventListener(IProgress<ICrawlActivityEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlErrorEventListener(IProgress<ICrawlErrorEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlManagerEventListener(IProgress<ICrawlManagerEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileCrawlEventListener(IProgress<FileCrawlEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileSystemItemEventListener(IProgress<ICrawlManagerFsItemEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSubdirectoryCrawlEventListener(IProgress<DirectoryCrawlEventArgs> listener)
        {
            throw new NotImplementedException();
        }

        private CrawlJob QueueCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt)
        {
            throw new NotImplementedException();
        }

        ICrawlJob ICrawlManagerService.QueueCrawlAsync(ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt) => QueueCrawlAsync(crawlConfiguration, stopAt);

        ICrawlJob ICrawlManagerService.QueueCrawlAsync(ILocalCrawlConfiguration crawlConfiguration) => QueueCrawlAsync(crawlConfiguration, null);
    }
}
