namespace FsInfoCat.Local.Crawling
{
    internal record FileCrawlContext
    {
        internal FileCrawlStartEventArgs EventArgs { get; init; }

        internal DirectoryCrawlContext Parent { get; init; }

        internal ulong ItemNumber { get; init; }
    }
}
