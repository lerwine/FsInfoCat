using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for entities that represent a subdirectory node within a file system.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="Local.ILocalSubdirectory" />
    /// <seealso cref="Upstream.IUpstreamSubdirectory" />
    public interface ISubdirectory : IDbFsItem
    {
        /// <summary>
        /// Gets or sets the crawl options for the current subdirectory.
        /// </summary>
        /// <value>The crawl options for the current subdirectory.</value>
        DirectoryCrawlOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the status of the current subdirectory.
        /// </summary>
        /// <value>The status value for the current subdirectory.</value>
        DirectoryStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the parent volume.
        /// </summary>
        /// <value>The parent volume (if this is the root subdirector) or <see langword="null"/> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null"/>, then <see cref="IDbFsItem.Parent"/> should not be null, and vice-versa.</remarks>
        IVolume Volume { get; set; }

        /// <summary>
        /// Gets or sets the crawl configuration that starts with the current subdirectory.
        /// </summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        ICrawlConfiguration CrawlConfiguration { get; set; }

        /// <summary>
        /// Gets the files directly contained within this subdirectory.
        /// </summary>
        /// <value>The files directly contained within this subdirectory.</value>
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Gets the nested subdirectories directly contained within this subdirectory.
        /// </summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        IEnumerable<ISubdirectory> SubDirectories { get; }

        /// <summary>
        /// Gets the access errors that occurred while attempting to access the current subdirectory.
        /// </summary>
        /// <value>The access errors that occurred while attempting to access the current subdirectory.</value>
        new IEnumerable<IAccessError<ISubdirectory>> AccessErrors { get; }
    }
}
