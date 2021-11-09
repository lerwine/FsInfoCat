using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlStartEventArgs : FileCrawlEventArgs
    {
        // TODO: Create constructor that does not use ICrawlJob parameter
        public FileCrawlStartEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target) : base(source, target, MessageCode.ReadingFileInformation) { }
    }
}
