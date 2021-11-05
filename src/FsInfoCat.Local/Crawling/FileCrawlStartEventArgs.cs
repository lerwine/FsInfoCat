using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlStartEventArgs : FileCrawlEventArgs
    {
        public FileCrawlStartEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target) : base(source, target, MessageCode.ReadingFileInformation) { }
    }
}
