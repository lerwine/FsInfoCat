using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for a personal (private) tag list item entity that can be associated with <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinition" />
    /// <seealso cref="IEquatable{IPersonalTagDefinition}" />
    public interface IPersonalTagDefinition : ITagDefinition, IEquatable<IPersonalTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="IPersonalFileTag"/> entities that associate <see cref="IFile"/> entities with the current entity.</value>
        new IEnumerable<IPersonalFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="IPersonalSubdirectoryTag"/> entities that associate <see cref="ISubdirectory"/> entities with the current entity.</value>
        new IEnumerable<IPersonalSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="IPersonalVolumeTag"/> entities that associate <see cref="IVolume"/> entities with the current entity.</value>
        new IEnumerable<IPersonalVolumeTag> VolumeTags { get; }
    }
}
