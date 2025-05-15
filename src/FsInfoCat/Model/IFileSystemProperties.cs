using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities that describe a file system.
    /// </summary>
    /// <seealso cref="IFileSystemRow" />
    public interface IFileSystemProperties
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name of the file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether this is a read-only file system type.
        /// </summary>
        /// <value><see langword="true" /> if this is a read-only file system type; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool ReadOnly { get; }

        /// <summary>
        /// Gets the maximum length of file system name components.
        /// </summary>
        /// <value>The maximum length of file system name components.</value>
        [Display(Name = nameof(Properties.Resources.MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint MaxNameLength { get; }

        /// <summary>
        /// Gets the default drive type for this file system.
        /// </summary>
        /// <value>The default drive type for this file system.</value>
        [Display(Name = nameof(Properties.Resources.DefaultDriveType), ResourceType = typeof(Properties.Resources))]
        DriveType? DefaultDriveType { get; }
    }
}
