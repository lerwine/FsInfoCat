using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Base interface for a database entity that represents a file system node.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IFile" />
    /// <seealso cref="ISubdirectory" />
    /// <seealso cref="Local.Model.ILocalDbFsItem" />
    /// <seealso cref="Upstream.Model.IUpstreamDbFsItem" />
    public interface IDbFsItem : IDbFsItemRow
    {
        /// <summary>
        /// Gets the parent subdirectory of the current file system item.
        /// </summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Parent { get; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The <see cref="IAccessError">access errors</see> for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IAccessError> AccessErrors { get; }

        /// <summary>
        /// Gets the personal tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="IPersonalTag"/> entities that associate <see cref="IPersonalTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IPersonalTag> PersonalTags { get; }

        /// <summary>
        /// Gets the shared tags associated with the current file system item.
        /// </summary>
        /// <value>The <see cref="ISharedTag"/> entities that associate <see cref="ISharedTagDefinition"/> entities with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PersonalTags), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISharedTag> SharedTags { get; }

        /// <summary>
        /// Attempts to get the primary key of the parent subdirectory.
        /// </summary>
        /// <param name="subdirectoryId">The <see cref="IHasSimpleIdentifier.Id"/> value of the parent <see cref="ISubdirectoryRow"/>.</param>
        /// <returns><see langword="true"/> if the current file system item has a parent <see cref="ISubdirectoryRow"/>; otherwise, <see langword="false"/>.</returns>
        bool TryGetParentId(out Guid subdirectoryId);
    }
}
