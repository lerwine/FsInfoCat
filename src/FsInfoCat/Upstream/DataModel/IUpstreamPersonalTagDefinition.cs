using System;
using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a personal (private) tag list item entity that can be associated with <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/> entities.
    /// </summary>
    /// <seealso cref="IUpstreamTagDefinition" />
    /// <seealso cref="IPersonalTagDefinition" />
    /// <seealso cref="IEquatable{IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="Local.ILocalPersonalTagDefinition" />
    public interface IUpstreamPersonalTagDefinition : IUpstreamTagDefinition, IPersonalTagDefinition, IEquatable<IUpstreamPersonalTagDefinition>
    {
        /// <summary>
        /// Gets the user who owns the tag definition.
        /// </summary>
        /// <value>The <see cref="IUserProfile"/> entity for the user who owns the tag.</value>
        IUserProfile User { get; }

        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamPersonalFileTag"/> entities that associate <see cref="IUpstreamFile"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamPersonalFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamPersonalSubdirectoryTag"/> entities that associate <see cref="IUpstreamSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamPersonalSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamPersonalVolumeTag"/> entities that associate <see cref="IUpstreamVolume"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamPersonalVolumeTag> VolumeTags { get; }
    }
}
