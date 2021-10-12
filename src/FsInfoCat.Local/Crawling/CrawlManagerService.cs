using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    internal class CrawlManagerService : ICrawlManagerService
    {
        public bool IsActive => throw new NotImplementedException();

        public void AddCrawlActivityEventListener([DisallowNull] ICrawlActivityEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void AddCrawlErrorEventListener([DisallowNull] ICrawlErrorEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void AddCrawlManagerEventListener([DisallowNull] ICrawlManagerEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void AddFileCrawlEventListener([DisallowNull] IFileCrawlEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void AddFileSystemItemEventListener([DisallowNull] IFileSystemItemEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void AddSubdirectoryCrawlEventListener([DisallowNull] ISubdirectoryCrawlEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void CancelAllCrawlsAsync()
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlActivityEventListener(ICrawlActivityEventListener listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlErrorEventListener(ICrawlErrorEventListener listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCrawlManagerEventListener(ICrawlManagerEventListener listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileCrawlEventListener(IFileCrawlEventListener listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileSystemItemEventListener(IFileSystemItemEventListener listener)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSubdirectoryCrawlEventListener(ISubdirectoryCrawlEventListener listener)
        {
            throw new NotImplementedException();
        }

        internal CrawlJob StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime? stopAt = null)
        {
            throw new NotImplementedException();
        }

        ICrawlJob ICrawlManagerService.StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration, DateTime stopAt) => StartCrawlAsync(crawlConfiguration, stopAt);

        ICrawlJob ICrawlManagerService.StartCrawlAsync([DisallowNull] ILocalCrawlConfiguration crawlConfiguration) => StartCrawlAsync(crawlConfiguration, null);
    }
}
