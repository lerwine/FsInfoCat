using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlEndEventArgs : FileCrawlEventArgs
    {
        // TODO: Create constructor that does not use ICrawlJob parameter
        public FileCrawlEndEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target, MessageCode statusDescription)
            : base(source, target, statusDescription) { }
    }
}
