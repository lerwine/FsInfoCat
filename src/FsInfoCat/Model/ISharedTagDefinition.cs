using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a shared tag list item entity that can be associated with <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/> entities.
    /// </summary>
    /// <seealso cref="IPersonalTagDefinition" />
    /// <seealso cref="IDbContext.SharedTagDefinitions"/>
    public interface ISharedTagDefinition : ITagDefinition, IEquatable<ISharedTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="ISharedFileTag"/> entities that associate <see cref="IFile"/> entities with the current entity.</value>
        new IEnumerable<ISharedFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="ISharedSubdirectoryTag"/> entities that associate <see cref="ISubdirectory"/> entities with the current entity.</value>
        new IEnumerable<ISharedSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="ISharedVolumeTag"/> entities that associate <see cref="IVolume"/> entities with the current entity.</value>
        new IEnumerable<ISharedVolumeTag> VolumeTags { get; }
    }
}
