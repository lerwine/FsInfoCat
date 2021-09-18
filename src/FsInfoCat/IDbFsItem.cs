using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{

    /// <summary>Base interface for a database entity that represents a file system node.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IDbFsItem : IDbFsItemRow
    {
        /// <summary>Gets the parent subdirectory of the current file system item.</summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Parent { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IAccessError> AccessErrors { get; }

        IEnumerable<IPersonalTag> PersonalTags { get; }

        IEnumerable<ISharedTag> SharedTags { get; }
    }
}

