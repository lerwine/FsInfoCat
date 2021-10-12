using FsInfoCat.Local.Crawling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlManagerService : ICrawlManagerService
    {
        private readonly LinkedList<WeakReference<ICrawlActivityEventListener>> _crawlActivityEventListeners = new();
        private readonly LinkedList<WeakReference<ICrawlErrorEventListener>> _crawlErrorEventListeners = new();
        private readonly LinkedList<WeakReference<ICrawlManagerEventListener>> _crawlManagerEventListeners = new();
        private readonly LinkedList<WeakReference<IFileSystemItemEventListener>> _fileSystemItemEventListeners = new();
        private readonly LinkedList<WeakReference<ISubdirectoryCrawlEventListener>> _subdirectoryCrawlEventListeners = new();
        private readonly LinkedList<WeakReference<IFileCrawlEventListener>> _fileCrawlEventListeners = new();
        private readonly LinkedList<CrawlJob> _pendingJobs = new();
        private CrawlJob _activeJob;
        private readonly IFileSystemDetailService _fileSystemDetailService;
        private readonly ILogger<CrawlManagerService> _logger;

        public bool IsActive { get; private set; }

        private static IEnumerable<T> GetItems<T>(LinkedList<WeakReference<T>> list) where T : class
        {
            LinkedListNode<WeakReference<T>> node = list.First;
            while (node is not null)
            {
                LinkedListNode<WeakReference<T>> next = node.Next;
                if (node.Value.TryGetTarget(out T target))
                    yield return target;
                else
                    list.Remove(node);
                node = next;
            }
        }

        private static T[] GetItemArray<T>(LinkedList<WeakReference<T>> list)
            where T : class
        {
            T[] result;
            lock (list)
                result = GetItems(list).ToArray();
            return result;
        }

        private static bool AddItem<T>(LinkedList<WeakReference<T>> list, T item) where T : class
        {
            if (item is null)
                return false;
            lock (list)
            {
                if (GetItems(list).Any(i => ReferenceEquals(i, item)))
                    return false;
                list.AddLast(new WeakReference<T>(item));
            }
            return true;
        }

        private static bool RemoveItem<T>(LinkedList<WeakReference<T>> list, T item) where T : class
        {
            if (item is null)
                return false;
            lock (list)
            {
                LinkedListNode<WeakReference<T>> node = list.First;
                while (node is not null)
                {
                    LinkedListNode<WeakReference<T>> next = node.Next;
                    if (node.Value.TryGetTarget(out T target))
                    {
                        if (ReferenceEquals(target, item))
                        {
                            list.Remove(node);
                            return true;
                        }
                    }
                    else
                        list.Remove(node);
                    node = next;
                }
            }
            return false;
        }

        public void AddCrawlActivityEventListener([DisallowNull] ICrawlActivityEventListener listener) => AddItem(_crawlActivityEventListeners, listener);

        public bool RemoveCrawlActivityEventListener(ICrawlActivityEventListener listener) => RemoveItem(_crawlActivityEventListeners, listener);

        public void AddCrawlManagerEventListener([DisallowNull] ICrawlManagerEventListener listener) => AddItem(_crawlManagerEventListeners, listener);

        public bool RemoveCrawlManagerEventListener(ICrawlManagerEventListener listener) => RemoveItem(_crawlManagerEventListeners, listener);

        public void AddCrawlErrorEventListener([DisallowNull] ICrawlErrorEventListener listener) => AddItem(_crawlErrorEventListeners, listener);

        public bool RemoveCrawlErrorEventListener(ICrawlErrorEventListener listener) => RemoveItem(_crawlErrorEventListeners, listener);

        public void AddFileSystemItemEventListener([DisallowNull] IFileSystemItemEventListener listener) => AddItem(_fileSystemItemEventListeners, listener);

        public bool RemoveFileSystemItemEventListener(IFileSystemItemEventListener listener) => RemoveItem(_fileSystemItemEventListeners, listener);

        public void AddSubdirectoryCrawlEventListener([DisallowNull] ISubdirectoryCrawlEventListener listener) => AddItem(_subdirectoryCrawlEventListeners, listener);

        public bool RemoveSubdirectoryCrawlEventListener(ISubdirectoryCrawlEventListener listener) => RemoveItem(_subdirectoryCrawlEventListeners, listener);

        public void AddFileCrawlEventListener([DisallowNull] IFileCrawlEventListener listener) => AddItem(_fileCrawlEventListeners, listener);

        public bool RemoveFileCrawlEventListener(IFileCrawlEventListener listener) => RemoveItem(_fileCrawlEventListeners, listener);

        public ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt) => throw new NotImplementedException();

        public ICrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration) => throw new NotImplementedException();

        public void CancelAllCrawlsAsync()
        {
            throw new NotImplementedException();
        }

        public CrawlManagerService(IFileSystemDetailService fileSystemDetailService, ILogger<CrawlManagerService> logger)
        {
            _fileSystemDetailService = fileSystemDetailService;
            _logger = logger;
        }

        private void RaiseCrawlJobStart(CrawlJobStartEventArgs args)
        {
            if (args.IsFirstJob)
                IsActive = true;
            foreach (ICrawlActivityEventListener listener in GetItemArray(_crawlActivityEventListeners))
                listener.OnCrawlActivity(args);
            foreach (ICrawlManagerEventListener listener in GetItemArray(_crawlManagerEventListeners))
                listener.OnCrawlManagerEvent(args);
        }

        private void RaiseCrawlJobEnd(CrawlJobEndEventArgs args)
        {
            if (args.IsLastJob)
                IsActive = false;
            foreach (ICrawlActivityEventListener listener in GetItemArray(_crawlActivityEventListeners))
                listener.OnCrawlActivity(args);
            foreach (ICrawlManagerEventListener listener in GetItemArray(_crawlManagerEventListeners))
                listener.OnCrawlManagerEvent(args);
        }

        private void RaiseDirectoryCrawling(DirectoryCrawlEventArgs args)
        {

        }

        private void RaiseDirectoryCrawled(DirectoryCrawlEventArgs args)
        {

        }

        private void RaiseFileCrawling(FileCrawlEventArgs args)
        {

        }

        private void RaiseFileCrawled(FileCrawlEventArgs args)
        {

        }
    }
}
