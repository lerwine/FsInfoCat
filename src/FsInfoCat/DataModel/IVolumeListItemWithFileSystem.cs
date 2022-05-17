using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume and contains associated file system properties.
    /// </summary>
    /// <seealso cref="IVolumeListItem" />
    /// <seealso cref="IEquatable{IVolumeListItemWithFileSystem}" />
    /// <seealso cref="Local.ILocalVolumeListItemWithFileSystem" />
    /// <seealso cref="Upstream.IUpstreamVolumeListItemWithFileSystem" />
    public interface IVolumeListItemWithFileSystem : IVolumeListItem, IEquatable<IVolumeListItemWithFileSystem>
    {
        /// <summary>
        /// Gets the display name of the volume's file system.
        /// </summary>
        /// <value>The <see cref="IFileSystemProperties.DisplayName"/> of the associated filesystem.</value>
        string FileSystemDisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether the volume is effectively read-only.
        /// </summary>
        /// <value><see langword="true"/> if the volume properties or the associated file system type indicates this is read-only; otherwise, <see langword="false"/>.</value>
        bool EffectiveReadOnly { get; }

        /// <summary>
        /// Gets the effective maximum name length.
        /// </summary>
        /// <value>The maximum name length taken from the volume properties or file system type.</value>
        uint EffectiveMaxNameLength { get; }
    }
}
