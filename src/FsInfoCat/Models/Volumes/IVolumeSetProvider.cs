using FsInfoCat.Models.HostDevices;
using FsInfoCat.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Models.Volumes
{
    public interface IVolumeSetProvider : IEqualityComparer<FileUri>, ICollection
    {
        bool TryFind(FileUri uri, out IVolumeInfo value);
        bool TryFindByRootURI(FileUri uri, out IVolumeInfo value);
        bool TryFindByName(string volumeName, out IVolumeInfo value);
    }

    public interface IVolumeSetProvider<V> : IVolumeSetProvider, ICollection<V>, IReadOnlyDictionary<VolumeIdentifier, V>
        where V : IVolumeInfo
    {
        bool TryFind(FileUri uri, out V value);
        bool TryFindByRootURI(FileUri uri, out V value);
        bool TryFindByName(string volumeName, out V value);
    }

    public interface IVolumeSetProvider<V, H> : IVolumeSetProvider<V>
        where H : IHostDevice
        where V : IVolume<H>
    {
    }
}
