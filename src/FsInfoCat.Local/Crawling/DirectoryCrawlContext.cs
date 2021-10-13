using System.Threading;

namespace FsInfoCat.Local.Crawling
{
    internal record DirectoryCrawlContext
    {
        internal LocalDbContext DbContext { get; init; }

        internal DirectoryCrawlStartEventArgs EventArgs { get; init; }

        internal CancellationToken Token { get; init; }

        internal ushort Depth { get; init; }

        internal ulong ItemNumber { get; init; }
    }
}
