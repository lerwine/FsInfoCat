using FsInfoCat.Models.HostDevices;
using FsInfoCat.Util;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Models.Volumes
{
    /// <summary>
    /// Associates a set of <seealso cref="IVolumeInfo"/> objects which can be referenced using a <seealso cref="FileUri"/> object.
    /// </summary>
    public interface IVolumeSetProvider : IEqualityComparer<FileUri>, ICollection
    {
        bool ContainsRootUri(FileUri uri);

        bool ContainsVolumeName(string name);

        /// <summary>
        /// Looks for a <seealso cref="IVolumeInfo"/> whose <seealso cref="IVolumeInfo.RootUri"/> is equal to or is a parent of the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">A <seealso cref="FileUri"/> that is a hierarchical member of a volume.</param>
        /// <param name="value">Returns he matching <seealso cref="IVolumeInfo"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        bool TryFind(FileUri uri, out IVolumeInfo value);

        /// <summary>
        /// Looks for a <seealso cref="IVolumeInfo"/> where the <seealso cref="IVolumeInfo.RootUri"/> is equal to the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">The <seealso cref="FileUri"/> which represents the volume root path.</param>
        /// <param name="value">Returns he matching <seealso cref="IVolumeInfo"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        bool TryFindByRootURI(FileUri uri, out IVolumeInfo value);

        /// <summary>
        /// Looks up a <seealso cref="IVolumeInfo"/> by its <seealso cref="IVolumeInfo.VolumeName"/>.
        /// </summary>
        /// <param name="volumeName">The case-insensitive volume name to look up.</param>
        /// <param name="value">Returns a <seealso cref="IVolumeInfo"/> whose <seealso cref="IVolumeInfo.VolumeName"/> matches given
        /// <seealso cref="VolumeIdentifier"/> <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        bool TryFindByName(string volumeName, out IVolumeInfo value);

        bool TryFindMatching(IVolumeInfo item, out IVolumeInfo actual);
    }

    public interface IVolumeSetProvider<V> : IVolumeSetProvider, ICollection<V>, IReadOnlyDictionary<VolumeIdentifier, V>
        where V : IVolumeInfo
    {
        bool TryFind(FileUri uri, out V value);
        bool TryFindByRootURI(FileUri uri, out V value);
        bool TryFindByName(string volumeName, out V value);
        bool TryFindMatching(IVolumeInfo item, out V actual);
    }

    public interface IVolumeSetProvider<V, H> : IVolumeSetProvider<V>
        where H : IHostDevice
        where V : IVolumeRecord<H>
    {
    }
}
