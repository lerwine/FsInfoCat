using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/> entities.
    /// </summary>
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="Local.Model.ILocalTagDefinition" />
    /// <seealso cref="Upstream.Model.IUpstreamTagDefinition" />
    /// <seealso cref="IItemTag.Definition" />
    public interface ITagDefinition : ITagDefinitionRow, IEquatable<ITagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="IFileTag"/> entities that associate <see cref="IFile"/> entities with the current entity.</value>
        IEnumerable<IFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="ISubdirectoryTag"/> entities that associate <see cref="ISubdirectory"/> entities with the current entity.</value>
        IEnumerable<ISubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="IVolumeTag"/> entities that associate <see cref="IVolume"/> entities with the current entity.</value>
        IEnumerable<IVolumeTag> VolumeTags { get; }
    }
}
