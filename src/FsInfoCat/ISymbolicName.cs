using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface ISymbolicNameRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the symbolic name.</summary>
        /// <value>The symbolic name which refers to a file system type..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets the custom notes for the current symbolic name.</summary>
        /// <value>The custom notes to associate with the current symblic name.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the priority for this symbolic name.</summary>
        /// <value>The priority of this symbolic name in relation to other symbolic names that refer to the same file system type, with lower values being higher priority.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        int Priority { get; }

        /// <summary>Gets a value indicating whether this symbolic name is inactive.</summary>
        /// <value><see langword="true" /> if this symbolic name  is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        Guid FileSystemId { get; }
    }
    public interface ISymbolicNameListItem : ISymbolicNameRow
    {
        string FileSystemDisplayName { get; }
    }
    /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
    /// <seealso cref="IDbEntity" />
    public interface ISymbolicName : ISymbolicNameRow
    {
        /// <summary>Gets the file system that this symbolic name refers to.</summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }
    }

}

