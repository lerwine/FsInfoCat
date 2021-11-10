using FsInfoCat.Services;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlStartEventArgs : FileCrawlEventArgs
    {
        public FileCrawlStartEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentFile file)
            : base(bgOperation, file, MessageCode.ReadingFileInformation, file.GetFullName()) { }
    }
}
