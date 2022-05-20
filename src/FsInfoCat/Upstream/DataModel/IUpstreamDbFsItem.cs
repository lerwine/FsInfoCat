using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Base interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="Local.ILocalDbFsItem" />
    /// <seealso cref="IUpstreamFile" />
    /// <seealso cref="IUpstreamSubdirectory" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDbFsItem")]
    public interface IUpstreamDbFsItem : IUpstreamDbFsItemRow, IDbFsItem
    {
        /// <summary>
        /// Gets the parent subdirectory.
        /// </summary>
        /// <value>The parent <see cref="IUpstreamSubdirectory" /> or <see langword="null" /> if this is the root <see cref="IUpstreamSubdirectory" />.</value>
        /// <remarks>
        /// If the current entity is a <see cref="IUpstreamSubdirectory" /> and this is <see langword="null" />,
        /// then <see cref="IUpstreamSubdirectory.Volume" /> should not be <see langword="null" />, and vice-versa;
        /// otherwise, if the current entity is a <see cref="IUpstreamFile" />, then this should never be <see langword="null" />.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Parent { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="IUpstreamPersonalTag"/> entities that associate <see cref="IUpstreamPersonalTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamPersonalTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="IUpstreamSharedTag"/> entities that associate <see cref="IUpstreamSharedTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSharedTag> SharedTags { get; }
    }
}
