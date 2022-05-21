using FsInfoCat.Model;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Local.Model
{

    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/> or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalTagDefinitionRow" />
    /// <seealso cref="ITagDefinition" />
    /// <seealso cref="IEquatable{ILocalTagDefinition}" />
    /// <seealso cref="Upstream.Model.IUpstreamTagDefinition" />
    public interface ILocalTagDefinition : ILocalTagDefinitionRow, ITagDefinition, IEquatable<ILocalTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="ILocalFileTag"/> entities that associate <see cref="ILocalFile"/> entities with the current entity.</value>
        new IEnumerable<ILocalFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="ILocalSubdirectoryTag"/> entities that associate <see cref="ILocalSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<ILocalSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="ILocalVolumeTag"/> entities that associate <see cref="ILocalVolume"/> entities with the current entity.</value>
        new IEnumerable<ILocalVolumeTag> VolumeTags { get; }
    }
}
