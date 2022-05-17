using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for list item entities which represent a logical file system volume.
    /// </summary>
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="IEquatable{IVolumeListItem}" />
    /// <seealso cref="Local.ILocalVolumeListItem" />
    /// <seealso cref="Upstream.IUpstreamVolumeListItem" />
    public interface IVolumeListItem : IVolumeRow, IEquatable<IVolumeListItem>
    {
        /// <summary>
        /// Gets the path of the volume's root subdirectory.
        /// </summary>
        /// <value>The full path of the volume's root <see cref="ISubdirectory"/>.</value>
        string RootPath { get; }

        /// <summary>
        /// Gets the root subdirectory count.
        /// </summary>
        /// <value>The number of subdirectories directly inside the volume's root <see cref="ISubdirectory"/>.</value>
        long RootSubdirectoryCount { get; }

        /// <summary>
        /// Gets the root subdirectory file count.
        /// </summary>
        /// <value>The number of files directly inside the volume's root <see cref="ISubdirectory"/>.</value>
        long RootFileCount { get; }

        /// <summary>
        /// Gets the access error count.
        /// </summary>
        /// <value>Gets the number of access errors that occurred while attempting to access the current volume entity.</value>
        long AccessErrorCount { get; }

        /// <summary>
        /// Gets the shared tag count.
        /// </summary>
        /// <value>The number shared tags associated with the current volume entity.</value>
        long SharedTagCount { get; }

        /// <summary>
        /// Gets the personal tag count.
        /// </summary>
        /// <value>The number personal personal tags associated with the current volume entity.</value>
        long PersonalTagCount { get; }
    }
}
