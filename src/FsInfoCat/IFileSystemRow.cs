using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IFileSystemRow : IFileSystemProperties, IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the custom notes for this file system type.</summary>
        /// <value>The custom notes to associate with this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets a value indicating whether this file system type is inactive.</summary>
        /// <value><see langword="true" /> if this file system type is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
}

