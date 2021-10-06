using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlErrorEventArgs : ICrawlActivityEventArgs
    {
        Exception Exception { get; }
    }
}
