using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlEndEventArgs : FileCrawlEventArgs
    {
        public FileCrawlEndEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target, MessageCode statusDescription)
            : base(source, target, statusDescription) { }
    }
}
