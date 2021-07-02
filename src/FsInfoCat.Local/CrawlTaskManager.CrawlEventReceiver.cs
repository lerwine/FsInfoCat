using System;
using System.IO;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlTaskManager
    {
        public class CrawlEventReceiver
        {
            public event EventHandler<CrawlEventArgs> CrawlStarted;
            public event EventHandler<CrawlEventArgs> CrawlCanceled;
            public event EventHandler<CrawlErrorEventArgs> CrawlFaulted;
            public event EventHandler<CrawlEventArgs> CrawlFinished;
            public event EventHandler<DirectoryCrawlEventArgs> EnterSubdirectory;
            public event EventHandler<DirectoryCrawlErrorEventArgs> SubdirectoryAccessError;
            public event EventHandler<DirectoryCrawlEventArgs> ExitSubdirectory;
            public event EventHandler<FileCrawlEventArgs> ReadingFile;
            public event EventHandler<FileCrawlErrorEventArgs> FileAccessError;
            public event EventHandler<FileCrawlEventArgs> FileReadComplete;

            internal void RaiseCrawlStarted(CrawlTaskManager crawlWorker) => OnCrawlStarted(new CrawlEventArgs(crawlWorker));

            internal void RaiseCrawlCanceled(CrawlTaskManager crawlWorker) => OnCrawlCanceled(new CrawlEventArgs(crawlWorker));

            internal void RaiseCrawlFaulted(CrawlTaskManager crawlWorker, AggregateException exception) => OnCrawlFaulted(new CrawlErrorEventArgs(crawlWorker, exception));

            internal void RaiseCrawlFinished(CrawlTaskManager crawlWorker) => OnCrawlFinished(new CrawlEventArgs(crawlWorker));

            internal void RaiseEnterSubdirectory(CrawlContext crawlContext) => OnEnterSubdirectory(new DirectoryCrawlEventArgs(crawlContext));

            internal void RaiseExitSubdirectory(CrawlContext crawlContext) => OnExitSubdirectory(new DirectoryCrawlEventArgs(crawlContext));

            internal void RaiseSubdirectoryAccessError(CrawlContext crawlContext, AccessErrorCode errorCode, Exception exception) =>
                OnSubdirectoryAccessError(new DirectoryCrawlErrorEventArgs(crawlContext, errorCode, exception));

            internal void RaiseReadingFile(CrawlContext crawlContext, FileInfo fs, DbFile db) => OnReadingFile(new FileCrawlEventArgs(crawlContext, fs, db));

            internal void RaiseFileReadComplete(CrawlContext crawlContext, FileInfo fs, DbFile db) => OnFileReadComplete(new FileCrawlEventArgs(crawlContext, fs, db));

            internal void RaiseFileAccessError(CrawlContext crawlContext, AccessErrorCode errorCode, FileInfo fs, DbFile db, Exception exception) =>
                OnFileAccessError(new FileCrawlErrorEventArgs(crawlContext, errorCode, fs, db, exception));

            protected virtual void OnCrawlStarted(CrawlEventArgs args) => CrawlStarted?.Invoke(this, args);

            protected virtual void OnCrawlCanceled(CrawlEventArgs args) => CrawlCanceled?.Invoke(this, args);

            protected virtual void OnCrawlFaulted(CrawlErrorEventArgs args) => CrawlFaulted?.Invoke(this, args);

            protected virtual void OnCrawlFinished(CrawlEventArgs args) => CrawlFinished?.Invoke(this, args);

            protected virtual void OnEnterSubdirectory(DirectoryCrawlEventArgs args) => EnterSubdirectory?.Invoke(this, args);

            protected virtual void OnExitSubdirectory(DirectoryCrawlEventArgs args) => ExitSubdirectory?.Invoke(this, args);

            protected virtual void OnSubdirectoryAccessError(DirectoryCrawlErrorEventArgs args) => SubdirectoryAccessError?.Invoke(this, args);

            protected virtual void OnReadingFile(FileCrawlEventArgs args) => ReadingFile?.Invoke(this, args);

            protected virtual void OnFileReadComplete(FileCrawlEventArgs args) => FileReadComplete?.Invoke(this, args);

            protected virtual void OnFileAccessError(FileCrawlErrorEventArgs args) => FileAccessError?.Invoke(this, args);
        }
    }
}
