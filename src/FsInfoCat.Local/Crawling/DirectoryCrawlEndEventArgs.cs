using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEndEventArgs : DirectoryCrawlEventArgs
    {
        // TODO: Create constructor that does not use ICrawlJob parameter
        public DirectoryCrawlEndEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target, MessageCode statusMessage)
            : base(source, target, statusMessage) { }
    }
}
