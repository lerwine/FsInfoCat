using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Base interface for a database entity that represents a file system node.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IDbFsItemRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the name of the current file system item.</summary>
        /// <value>The name of the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets the date and time last accessed.</summary>
        /// <value>The last accessed for the purposes of this application.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastAccessed), ResourceType = typeof(Properties.Resources))]
        DateTime LastAccessed { get; }

        /// <summary>Gets custom notes to be associated with the current file system item.</summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the file's creation time.</summary>
        /// <value>The creation time as reported by the host file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreationTime), ResourceType = typeof(Properties.Resources))]
        DateTime CreationTime { get; }

        /// <summary>Gets the date and time the file system item was last written nto.</summary>
        /// <value>The last write time as reported by the host file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastWriteTime), ResourceType = typeof(Properties.Resources))]
        DateTime LastWriteTime { get; }
    }

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
    public interface IDbFsItemAncestorName : IHasSimpleIdentifier
    {
        string Name { get; }

        string AncestorNames { get; }
    }
    public interface IDbFsItemListItem : IDbFsItemRow
    {
        long AccessErrorCount { get; }

        long PersonalTagCount { get; }

        long SharedTagCount { get; }
    }

    public interface IDbFsItemListItemWithAncestorNames : IDbFsItemListItem, IDbFsItemAncestorName { }
}

