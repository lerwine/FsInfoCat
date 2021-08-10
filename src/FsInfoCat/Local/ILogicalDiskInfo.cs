using System.IO;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Describes logical disks found on the local host machine.
    /// </summary>
    public interface ILogicalDiskInfo
    {
        /// <summary>
        /// Gets a value indicating whether the logical disk is read-only.
        /// </summary>
        /// <value><see langword="true"/> if the current logical disk is read-only ; otherwise, <see langword="false"/>.</value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The user-friendly display name of the logical disk.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ILogicalDiskInfo"/> is compressed.
        /// </summary>
        /// <value><see langword="true"/> if the current logical disk is compressed; otherwise,<see langword="false"/>.</value>
        bool Compressed { get; }

        /// <summary>
        /// Gets the drive type of the logical disk.
        /// </summary>
        /// <value>The drive type of the logical disk.</value>
        DriveType DriveType { get; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        /// <value>The name of the logical disk's file system.</value>
        string FileSystemName { get; }

        /// <summary>
        /// Gets the maximum length of file system names.
        /// </summary>
        /// <value>The maximum length of file system name components.</value>
        uint MaxNameLength { get; }

        /// <summary>
        /// Gets the name of the logical disk.
        /// </summary>
        /// <value>The name of the logical disk.</value>
        string Name { get; }

        /// <summary>
        /// Gets the name of the file system provider.
        /// </summary>
        /// <value>The name of the provider that is used to access the file system.</value>
        string ProviderName { get; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        /// <value>The name of the volume.</value>
        string VolumeName { get; }

        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        /// <value>The volume serial number.</value>
        string VolumeSerialNumber { get; }

        bool? ErrorCleared { get; }

        string ErrorDescription { get; }

        /// <summary>
        /// Gets the last error code reported by the logical device.
        /// </summary>
        /// <value>The last error code reported by the logical device.</value>
        uint? LastErrorCode { get; }

        /// <summary>
        /// Gets the root directory of the current volume.
        /// </summary>
        /// <value>The root directory of the current volume.</value>
        IVolumeDirectory RootDirectory { get; }
    }
}
