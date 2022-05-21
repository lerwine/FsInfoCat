using FsInfoCat.Model;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a tag entity that can be associated with <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/> or <see cref="IUpstreamVolume"/> entities.
    /// </summary>
    /// <seealso cref="IUpstreamTagDefinitionRow" />
    /// <seealso cref="ITagDefinition" />
    /// <seealso cref="IEquatable{IUpstreamTagDefinition}" />
    /// <seealso cref="Local.Model.ILocalTagDefinition" />
    public interface IUpstreamTagDefinition : IUpstreamTagDefinitionRow, ITagDefinition, IEquatable<IUpstreamTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamFileTag"/> entities that associate <see cref="IUpstreamFile"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamSubdirectoryTag"/> entities that associate <see cref="IUpstreamSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamVolumeTag"/> entities that associate <see cref="IUpstreamVolume"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamVolumeTag> VolumeTags { get; }
    }
}
