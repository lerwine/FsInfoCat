using FsInfoCat.Model;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a personal (private) tag list item entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalTagDefinition" />
    /// <seealso cref="IPersonalTagDefinition" />
    /// <seealso cref="IEquatable{ILocalPersonalTagDefinition}" />
    /// <seealso cref="Upstream.Model.IUpstreamPersonalTagDefinition" />
    public interface ILocalPersonalTagDefinition : ILocalTagDefinition, IPersonalTagDefinition, IEquatable<ILocalPersonalTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalFileTag"/> entities that associate <see cref="ILocalFile"/> entities with the current entity.</value>
        new IEnumerable<ILocalPersonalFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalSubdirectoryTag"/> entities that associate <see cref="ILocalSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<ILocalPersonalSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalVolumeTag"/> entities that associate <see cref="ILocalVolume"/> entities with the current entity.</value>
        new IEnumerable<ILocalPersonalVolumeTag> VolumeTags { get; }
    }
}
