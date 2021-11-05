using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlStartEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlStartEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target) : base(source, target, MessageCode.CrawlingSubdirectory) { }
    }
}
