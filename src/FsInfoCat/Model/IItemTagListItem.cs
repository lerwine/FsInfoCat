using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="ITagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="IEquatable{IItemTagListItem}" />
    /// <seealso cref="Local.Model.ILocalItemTagListItem" />
    /// <seealso cref="Upstream.Model.IUpstreamItemTagListItem" />
    /// <seealso cref="IDbContext.PersonalVolumeTagListing" />
    /// <seealso cref="IDbContext.SharedVolumeTagListing" />
    /// <seealso cref="IDbContext.PersonalSubdirectoryTagListing" />
    /// <seealso cref="IDbContext.SharedSubdirectoryTagListing" />
    /// <seealso cref="IDbContext.PersonalFileTagListing" />
    /// <seealso cref="IDbContext.SharedFileTagListing" />
    public interface IItemTagListItem : IItemTagRow, IEquatable<IItemTagListItem>
    {
        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        /// <value>The <see cref="ITagDefinitionRow.Name"/> of the tag.</value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the tag.
        /// </summary>
        /// <value>The <see cref="ITagDefinitionRow.Description"/> of the tag.</value>
        string Description { get; }
    }
}
