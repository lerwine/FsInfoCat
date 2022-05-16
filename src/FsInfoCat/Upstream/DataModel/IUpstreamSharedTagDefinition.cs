using System;
using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for a shared tag list item entity that can be associated with <see cref="IUpstreamFile"/>, <see cref="IUpstreamSubdirectory"/>
    /// or <see cref="IUpstreamVolume"/> entities.
    /// </summary>
    /// <seealso cref="IUpstreamTagDefinition" />
    /// <seealso cref="ISharedTagDefinition" />
    /// <seealso cref="IEquatable{IUpstreamSharedTagDefinition}" />
    /// <seealso cref="Local.ILocalSharedTagDefinition" />
    public interface IUpstreamSharedTagDefinition : IUpstreamTagDefinition, ISharedTagDefinition, IEquatable<IUpstreamSharedTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamSharedFileTag"/> entities that associate <see cref="IUpstreamFile"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamSharedFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamSharedSubdirectoryTag"/> entities that associate <see cref="IUpstreamSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamSharedSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="IUpstreamSharedVolumeTag"/> entities that associate <see cref="IUpstreamVolume"/> entities with the current entity.</value>
        new IEnumerable<IUpstreamSharedVolumeTag> VolumeTags { get; }
    }
}
