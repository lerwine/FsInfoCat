using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    public interface IVolumeRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the display name of the volume.
        /// </summary>
        /// <value>The display name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        /// <value>The name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(Properties.Resources))]
        string VolumeName { get; }

        /// <summary>
        /// Gets the unique volume identifier.
        /// </summary>
        /// <value>The system-independent unique identifier, which identifies the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Identifier), ResourceType = typeof(Properties.Resources))]
        VolumeIdentifier Identifier { get; }

        /// <summary>
        /// Gets a value indicating whether the current volume is read-only.
        /// </summary>
        /// <value><see langword="true" /> if the current volume is read-only; <see langword="false" /> if it is read/write; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystemProperties.ReadOnly">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool? ReadOnly { get; }

        /// <summary>
        /// Gets the maximum length of file system name components.
        /// </summary>
        /// <value>The maximum length of file system name components or <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystemProperties.MaxNameLength">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint? MaxNameLength { get; }

        /// <summary>
        /// Gets the drive type for this volume.
        /// </summary>
        /// <value>The drive type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Type), ResourceType = typeof(Properties.Resources))]
        DriveType Type { get; }

        /// <summary>
        /// Gets the custom notes for this volume.
        /// </summary>
        /// <value>The custom notes to associate with this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Gets the volume status.
        /// </summary>
        /// <value>The volume status value.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        VolumeStatus Status { get; }

        /// <summary>
        /// Gets the unique identifier of the entity host file system.
        /// </summary>
        /// <value>The unique identifier of the entity that represents the host file system for the current volume.</value>
        Guid FileSystemId { get; }
    }
}
