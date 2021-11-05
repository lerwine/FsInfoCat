using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEndEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlEndEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target, MessageCode statusMessage)
            : base(source, target, statusMessage) { }
    }
}
