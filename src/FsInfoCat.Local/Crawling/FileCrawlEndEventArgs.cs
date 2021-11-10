using FsInfoCat.Services;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlEndEventArgs : FileCrawlEventArgs
    {
        public FileCrawlEndEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentFile target, MessageCode statusMessage)
            : base(bgOperation, target, statusMessage, target.GetFullName()) { }
    }
}
