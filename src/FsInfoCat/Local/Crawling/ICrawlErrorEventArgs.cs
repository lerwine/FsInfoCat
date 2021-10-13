using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlErrorEventArgs : ICrawlManagerFsItemEventArgs
    {
        Exception Exception { get; }
    }
}
