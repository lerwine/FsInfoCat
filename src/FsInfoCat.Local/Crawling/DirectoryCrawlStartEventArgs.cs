using FsInfoCat.Services;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlStartEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlStartEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentDirectory directory)
            : base(bgOperation, directory, MessageCode.CrawlingSubdirectory, directory.GetFullName()) { }
    }
}
