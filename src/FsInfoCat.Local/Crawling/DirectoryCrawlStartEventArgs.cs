using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlStartEventArgs : DirectoryCrawlEventArgs
    {
        // TODO: Create constructor that does not use ICrawlJob parameter
        public DirectoryCrawlStartEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target) : base(source, target, MessageCode.CrawlingSubdirectory) { }
    }
}
