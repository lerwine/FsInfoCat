using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for entities which represent a specific file system type.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalFileSystem" />
    /// <seealso cref="Upstream.IUpstreamFileSystem" />
    public interface IFileSystem : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        /// <summary>Gets or sets the display name.</summary>
        /// <value>The display name of the file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether file name searches are case-sensitive.
        /// </summary>
        /// <value> <see langword="true"/> if file name searches are case-sensitive; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(Properties.Resources))]
        bool CaseSensitiveSearch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a read-only file system type.
        /// </summary>
        /// <value><see langword="true"/> if this is a read-only file system type; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of file system name components.
        /// </summary>
        /// <value>The maximum length of file system name components.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint MaxNameLength { get; set; }

        /// <summary>
        /// Gets or sets the default drive type for this file system.
        /// </summary>
        /// <value>The default drive type for this file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(Properties.Resources))]
        System.IO.DriveType? DefaultDriveType { get; set; }

        /// <summary>
        /// Gets or sets the custom notes for this file system type.
        /// </summary>
        /// <value>The custom notes to associate with this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this file system type is inactive.
        /// </summary>
        /// <value><see langword="true"/> if this file system type is inactive; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; set; }

        /// <summary>
        /// Gets the volumes that use this file system.
        /// </summary>
        /// <value>The volumes that use this file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolume> Volumes { get; }

        /// <summary>
        /// Gets the symbolic names of the current file system.
        /// </summary>
        /// <value>The symbolic names that are used to identify the current file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISymbolicName> SymbolicNames { get; }
    }
}
