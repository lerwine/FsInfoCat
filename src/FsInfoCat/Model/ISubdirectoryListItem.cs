using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a database list item entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IDbFsItemListItemWithAncestorNames" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="IDbContext.SubdirectoryListing"/>
    public interface ISubdirectoryListItem : IDbFsItemListItem, ISubdirectoryRow, IEquatable<ISubdirectoryListItem>
    {
        /// <summary>
        /// Gets the subdirectory count.
        /// </summary>
        /// <value>The number of immediate child subdirectory entities.</value>
        long SubdirectoryCount { get; }

        /// <summary>
        /// Gets the file count.
        /// </summary>
        /// <value>The number of files directly contained within the current subdirectory.</value>
        long FileCount { get; }

        /// <summary>
        /// Gets the display name of the crawl configuration using the current subdirectory and the crawl root.
        /// </summary>
        /// <value>The <see cref="ICrawlConfigurationRow.DisplayName"/> of the <see cref="ICrawlConfigurationRow"/> using the current subdirectory as the
        /// crawl <see cref="ICrawlConfiguration.Root"/> or an empty string if no crawl configuration uses the current subdirectory as its root.</value>
        string CrawlConfigDisplayName { get; }
    }
}
