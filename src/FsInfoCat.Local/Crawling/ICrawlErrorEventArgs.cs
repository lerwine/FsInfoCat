using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlErrorEventArgs : IFsItemCrawlEventArgs
    {
        Exception Exception { get; }
    }
}
