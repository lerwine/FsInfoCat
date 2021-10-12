using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    internal class CrawlJob : ICrawlJob
    {
        public DateTime Started => throw new NotImplementedException();

        public ICurrentItem CurrentItem => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string Message => throw new NotImplementedException();

        public StatusMessageLevel MessageLevel => throw new NotImplementedException();

        public Guid ConcurrencyId => throw new NotImplementedException();

        public AsyncJobStatus JobStatus => throw new NotImplementedException();

        public Task Task => throw new NotImplementedException();

        public bool IsCancellationRequested => throw new NotImplementedException();

        public TimeSpan Duration => throw new NotImplementedException();

        public object AsyncState => throw new NotImplementedException();

        public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

        public bool CompletedSynchronously => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public void Cancel(bool throwOnFirstException)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
