using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a database entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="Local.Model.ILocalSubdirectoryRow" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectoryRow" />
    public interface ISubdirectoryRow : IDbFsItemRow
    {
        /// <summary>
        /// Gets the crawl options for the current subdirectory.
        /// </summary>
        /// <value>The crawl options for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Options), ResourceType = typeof(Properties.Resources))]
        DirectoryCrawlOptions Options { get; }

        /// <summary>
        /// Gets the status of the current subdirectory.
        /// </summary>
        /// <value>The status value for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        DirectoryStatus Status { get; }

        /// <summary>
        /// Gets the primary key of the parent <see cref="ISubdirectoryRow"/>.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> value of the parent <see cref="ISubdirectoryRow"/> or <see langword="null"/> if this has no parent
        /// subdirectory.</value>
        /// <remarks>If this is <see langword="null"/>, then <see cref="VolumeId"/> should have a value.</remarks>
        Guid? ParentId { get; }

        /// <summary>
        /// Gets the primary key of the parent <see cref="IVolumeRow"/>.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> value of the parent <see cref="IVolumeRow"/> or <see langword="null"/> if this is a nested subdirectory.</value>
        /// <remarks>If this is <see langword="null"/>, then <see cref="ParentId"/> should have a value.</remarks>
        Guid? VolumeId { get; }
    }
}
