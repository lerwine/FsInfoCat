namespace FsInfoCat
{
    /// <summary>
    /// Configuration of a file system crawl instance.
    /// </summary>
    public interface ICrawlConfiguration : IDbEntity
    {
        string DisplayName { get; set; }

        string Notes { get; set; }

        ISubdirectory Root { get; }

        ushort MaxRecursionDepth { get; set; }

        ulong MaxTotalItems { get; set; }

        bool IsInactive { get; set; }
    }
}
