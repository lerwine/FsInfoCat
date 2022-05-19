using System;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a crawl configuration list item entity.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="IEquatable{ICrawlConfigurationListItem}" />
    /// <seealso cref="ICrawlConfigReportItem" />
    /// <seealso cref="Upstream.IUpstreamCrawlConfigurationListItem" />
    /// <seealso cref="Local.ILocalCrawlConfigurationListItem" />
    /// <seealso cref="IDbContext.CrawlConfigListing" />
    [Obsolete("Use FsInfoCat.Model.ICrawlConfigurationListItem")]
    public interface ICrawlConfigurationListItem : ICrawlConfigurationRow, IEquatable<ICrawlConfigurationListItem>
    {
        /// <summary>
        /// Gets the root subdirectory ancestor path element names.
        /// </summary>
        /// <value>The result of a calculated column that contains the names of the root <see cref="ISubdirectory"/> path elements, separated by slash (<c>/</c>) characters,
        /// and in reverse order from typical file system path segments.</value>
        string AncestorNames { get; }

        /// <summary>
        /// Gets the primary key of the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="IVolumeRow"/> containing the
        /// root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        Guid VolumeId { get; }

        /// <summary>
        /// Gets the display name of the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.DisplayName"/> of the <see cref="IVolumeRow"/> containing the
        /// root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        string VolumeDisplayName { get; }

        /// <summary>
        /// Gets the name of the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.VolumeName"/> of the <see cref="IVolume"/> containing the root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        string VolumeName { get; }

        /// <summary>
        /// Gets the identifier of the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.Identifier"/> of the <see cref="IVolume"/> containing the root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        VolumeIdentifier VolumeIdentifier { get; }

        /// <summary>
        /// Gets the display name of the file system for the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="IFileSystemProperties.DisplayName"/> of the <see cref="IFileSystemProperties">Filesystem</see> for the <see cref="IVolume"/> containing the
        /// root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        string FileSystemDisplayName { get; }

        /// <summary>
        /// Gets the symbolic name of the file system for the volume containing the root subdirectory for the current crawl configuration.
        /// </summary>
        /// <value>The <see cref="ISymbolicNameRow.Name">symbolic name</see> of the <see cref="IFileSystemProperties">Filesystem</see> for the <see cref="IVolume"/> containing the
        /// root <see cref="ISubdirectory"/> for the current crawl configuration.</value>
        string FileSystemSymbolicName { get; }
    }
}
