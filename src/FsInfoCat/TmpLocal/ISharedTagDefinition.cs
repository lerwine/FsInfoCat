using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for a shared tag list item entity that can be associated with <see cref="ILocalFile"/>, <see cref="ILocalSubdirectory"/>
    /// or <see cref="ILocalVolume"/> entities.
    /// </summary>
    /// <seealso cref="ILocalTagDefinition" />
    /// <seealso cref="M.ISharedTagDefinition" />
    /// <seealso cref="IEquatable{ILocalSharedTagDefinition}" />
    /// <seealso cref="Upstream.Model.ISharedTagDefinition" />
    public interface ILocalSharedTagDefinition : ILocalTagDefinition, M.ISharedTagDefinition, IEquatable<ILocalSharedTagDefinition>
    {
        /// <summary>
        /// Gets the file tags.
        /// </summary>
        /// <value>The <see cref="ILocalSharedFileTag"/> entities that associate <see cref="ILocalFile"/> entities with the current entity.</value>
        new IEnumerable<ILocalSharedFileTag> FileTags { get; }

        /// <summary>
        /// Gets the subdirectory tags.
        /// </summary>
        /// <value>The <see cref="ILocalSharedSubdirectoryTag"/> entities that associate <see cref="ILocalSubdirectory"/> entities with the current entity.</value>
        new IEnumerable<ILocalSharedSubdirectoryTag> SubdirectoryTags { get; }

        /// <summary>
        /// Gets the volume tags.
        /// </summary>
        /// <value>The <see cref="ILocalSharedVolumeTag"/> entities that associate <see cref="ILocalVolume"/> entities with the current entity.</value>
        new IEnumerable<ILocalSharedVolumeTag> VolumeTags { get; }
    }
}
