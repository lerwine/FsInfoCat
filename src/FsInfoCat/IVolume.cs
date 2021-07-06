using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalVolume" />
    /// <seealso cref="Upstream.IUpstreamVolume" />
    public interface IVolume : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the volume.
        /// </summary>
        /// <value>The display name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the volume.
        /// </summary>
        /// <value>The name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string VolumeName { get; set; }

        /// <summary>
        /// Gets or sets the unique volume identifier.
        /// </summary>
        /// <value>The system-independent unique identifier, which identifies the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Identifier), ResourceType = typeof(Properties.Resources))]
        VolumeIdentifier Identifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether file name searches are case-sensitive.
        /// </summary>
        /// <value> <see langword="true"/> if file name searches are case-sensitive; <see langword="false"/> if they are case-insensitive; otherwise, <see langword="null"/> to assume the same value as defined by the <seealso cref="IFileSystem.CaseSensitiveSearch">file system type</seealso>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(Properties.Resources))]
        bool? CaseSensitiveSearch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current volume is read-only.
        /// </summary>
        /// <value><see langword="true"/> if the current volume is read-only; <see langword="false"/> if it is read/write; otherwise, <see langword="null"/> to assume the same value as defined by the <seealso cref="IFileSystem.ReadOnly">file system type</seealso>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool? ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of file system name components.
        /// </summary>
        /// <value>The maximum length of file system name components or <see langword="null"/> to assume the same value as defined by the <seealso cref="IFileSystem.MaxNameLength">file system type</seealso>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint? MaxNameLength { get; set; }

        /// <summary>
        /// Gets or sets the drive type for this volume.
        /// </summary>
        /// <value>The drive type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DriveType), ResourceType = typeof(Properties.Resources))]
        System.IO.DriveType Type { get; set; }

        /// <summary>
        /// Gets or sets the custom notes for this volume.
        /// </summary>
        /// <value>The custom notes to associate with this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets the volume status.
        /// </summary>
        /// <value>The volume status value.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus), ResourceType = typeof(Properties.Resources))]
        VolumeStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the file system type.
        /// </summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; set; }

        /// <summary>
        /// Gets the root directory of this volume.
        /// </summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory RootDirectory { get; }

        /// <summary>
        /// Gets the access errors for this volume.
        /// </summary>
        /// <value>The access errors that occurred while trying to access this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IAccessError<IVolume>> AccessErrors { get; }
    }
}
