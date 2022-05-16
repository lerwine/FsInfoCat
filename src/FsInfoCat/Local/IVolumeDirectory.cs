using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a subdirectory of a file system volume.
    /// </summary>
    /// <seealso cref="ILogicalDiskInfo.RootDirectory"/>
    public interface IVolumeDirectory
    {
        /// <summary>
        /// Gets the full path name.
        /// </summary>
        /// <value>The full path name of the folder.</value>
        string Name { get; }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>A short textual description of the subdirectory.</value>
        string Caption { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>A long textual description of the subdirectory.</value>
        string Description { get; }

        /// <summary>
        /// Gets the drive of this <see cref="IVolumeDirectory"/>.
        /// </summary>
        /// <value>The drive letter (including colon).</value>
        string Drive { get; }

        /// <summary>
        /// Gets the creation date of this <see cref="IVolumeDirectory"/>.
        /// </summary>
        /// <value>The creation date or <see langword="null"/> if unknown, not applicable or not specified.</value>
        DateTime? CreationDate { get; }

        /// <summary>
        /// Gets the date and time when the folder was last modified.
        /// </summary>
        /// <value>The date and time when the folder was last modified or <see langword="null"/> if unknown, not applicable or not specified.</value>
        DateTime? LastModified { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IVolumeDirectory"/> is hidden.
        /// </summary>
        /// <value><see langword="true"/> if the folder is hidden, <see langword="false"/> if it is not hidden; otherwise, <see langword="null"/> unknown,
        /// not applicable or not specified.</value>
        bool? Hidden { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IVolumeDirectory"/> is readable.
        /// </summary>
        /// <value><see langword="true"/> if the folder is readable, <see langword="false"/> if it is not readable; otherwise, <see langword="null"/> unknown,
        /// not applicable or not specified.</value>
        bool Readable { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IVolumeDirectory"/> is writeable.
        /// </summary>
        /// <value><see langword="true"/> if the folder is writeable, <see langword="false"/> if it is not writeable; otherwise, <see langword="null"/> unknown,
        /// not applicable or not specified.</value>
        bool? Writeable { get; }
    }
}
