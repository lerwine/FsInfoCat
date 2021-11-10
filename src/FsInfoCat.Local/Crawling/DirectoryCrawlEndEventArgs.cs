using FsInfoCat.Services;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEndEventArgs : DirectoryCrawlEventArgs
    {
        public DirectoryCrawlEndEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentDirectory target, MessageCode statusMessage)
            : base(bgOperation, target, statusMessage, target.GetFullName()) { }
    }
}
