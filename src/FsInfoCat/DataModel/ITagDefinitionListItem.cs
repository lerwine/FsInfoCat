using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for a tag list item entity that can be associated with <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="IEquatable{ITagDefinitionListItem}" />
    /// <seealso cref="Local.ILocalTagDefinitionListItem" />
    /// <seealso cref="Upstream.IUpstreamTagDefinitionListItem" />
    /// <seealso cref="IDbContext.PersonalTagDefinitionListing" />
    /// <seealso cref="IDbContext.SharedTagDefinitionListing" />
    [System.Obsolete("Use FsInfoCat.Model.ITagDefinitionListItem")]
    public interface ITagDefinitionListItem : ITagDefinitionRow, IEquatable<ITagDefinitionListItem>
    {
        /// <summary>
        /// Gets the tag file count.
        /// </summary>
        /// <value>The number of <see cref="IFile"/> entities associated with the current tag.</value>
        long FileTagCount { get; }

        /// <summary>
        /// Gets the tagged subdirectory count.
        /// </summary>
        /// <value>The number of <see cref="ISubdirectory"/> entities associated with the current tag.</value>
        long SubdirectoryTagCount { get; }

        /// <summary>
        /// Gets the tagged volume count.
        /// </summary>
        /// <value>The number of <see cref="IVolume"/> entities associated with the current tag.</value>
        long VolumeTagCount { get; }
    }
}
