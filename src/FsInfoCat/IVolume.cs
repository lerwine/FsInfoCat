using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat
{
    /// <summary>Interface for entities which represent a logical file system volume.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IVolume : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the display name of the volume.</summary>
        /// <value>The display name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets the name of the volume.</summary>
        /// <value>The name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(Properties.Resources))]
        string VolumeName { get; }

        /// <summary>Gets the unique volume identifier.</summary>
        /// <value>The system-independent unique identifier, which identifies the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Identifier), ResourceType = typeof(Properties.Resources))]
        VolumeIdentifier Identifier { get; }

        /// <summary>Gets a value indicating whether file name searches are case-sensitive.</summary>
        /// <value><see langword="true" /> if file name searches are case-sensitive; <see langword="false" /> if they are case-insensitive; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.CaseSensitiveSearch">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(Properties.Resources))]
        bool? CaseSensitiveSearch { get; }

        /// <summary>Gets a value indicating whether the current volume is read-only.</summary>
        /// <value><see langword="true" /> if the current volume is read-only; <see langword="false" /> if it is read/write; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.ReadOnly">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool? ReadOnly { get; }

        /// <summary>Gets the maximum length of file system name components.</summary>
        /// <value>The maximum length of file system name components or <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.MaxNameLength">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint? MaxNameLength { get; }

        /// <summary>Gets the drive type for this volume.</summary>
        /// <value>The drive type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Type), ResourceType = typeof(Properties.Resources))]
        DriveType Type { get; }

        /// <summary>Gets the custom notes for this volume.</summary>
        /// <value>The custom notes to associate with this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the volume status.</summary>
        /// <value>The volume status value.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        VolumeStatus Status { get; }

        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolumeAccessError> AccessErrors { get; }
    }

}

