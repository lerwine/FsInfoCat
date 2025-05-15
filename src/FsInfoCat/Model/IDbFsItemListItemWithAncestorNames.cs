using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for database view entities derived from the <see cref="IDbContext.Files"/> or <see cref="IDbContext.Subdirectories"/> table
    /// and also contains path names as well as columns from the volume and file system entities.
    /// </summary>
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="ISubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="IFileListItemWithBinaryProperties" />
    /// <seealso cref="ISubdirectoryListItem" />
    /// <seealso cref="IFileAncestorName" />
    /// <seealso cref="ISubdirectoryAncestorName" />
    public interface IDbFsItemListItemWithAncestorNames : IDbFsItemListItem, IDbFsItemAncestorName
    {
        /// <summary>
        /// Gets the primary key of the parent volume.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the parent <see cref="IVolumeRow"/>.</value>
        Guid EffectiveVolumeId { get; }

        /// <summary>
        /// Gets the display name of the parent volume.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.DisplayName"/> of the parent <see cref="IVolumeRow"/>.</value>
        string VolumeDisplayName { get; }

        /// <summary>
        /// Gets the name of the parent volume.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.VolumeName"/> of the parent <see cref="IVolumeRow"/>.</value>
        string VolumeName { get; }

        /// <summary>
        /// Gets the parent volume identifier.
        /// </summary>
        /// <value>The <see cref="IVolumeRow.Identifier"/> of the parent <see cref="IVolumeRow"/>.</value>
        VolumeIdentifier VolumeIdentifier { get; }

        /// <summary>
        /// Gets the display name of the file system for the parent volume.
        /// </summary>
        /// <value>The <see cref="IFileSystemProperties.DisplayName"/> of the <see cref="IFileSystemProperties">Filesystem</see> for the parent <see cref="IVolume"/>.</value>
        string FileSystemDisplayName { get; }

        /// <summary>
        /// Gets the symbolic name of the file system for the parent volume.
        /// </summary>
        /// <value>The <see cref="ISymbolicNameRow.Name">symbolic name</see> of the <see cref="IFileSystemProperties">Filesystem</see> for the parent <see cref="IVolume"/>.</value>
        string FileSystemSymbolicName { get; }
    }
}
