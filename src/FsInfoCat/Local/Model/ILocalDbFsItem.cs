using FsInfoCat.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base interface for a database entity that represents a file system node on the local host machine.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="ILocalFile" />
    /// <seealso cref="ILocalSubdirectory" />
    /// <seealso cref="Upstream.Model.IUpstreamDbFsItem" />
    public interface ILocalDbFsItem : ILocalDbFsItemRow, IDbFsItem
    {
        /// <summary>
        /// Gets the parent subdirectory.
        /// </summary>
        /// <value>The parent <see cref="ILocalSubdirectory" /> or <see langword="null" /> if this is the root <see cref="ILocalSubdirectory" />.</value>
        /// <remarks>
        /// If the current entity is a <see cref="ILocalSubdirectory" /> and this is <see langword="null" />,
        /// then <see cref="ILocalSubdirectory.Volume" /> should not be <see langword="null" />, and vice-versa;
        /// otherwise, if the current entity is a <see cref="ILocalFile" />, then this should never be <see langword="null" />.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Parent { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalTag"/> entities that associate <see cref="ILocalPersonalTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalPersonalTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="ILocalSharedTag"/> entities that associate <see cref="ILocalSharedTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSharedTag> SharedTags { get; }
    }
}
