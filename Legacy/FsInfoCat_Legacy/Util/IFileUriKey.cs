using FsInfoCat.Models.Volumes;
using System;

namespace FsInfoCat.Util
{
    public interface IFileUriKey : IEquatable<IFileUriKey>
    {
        FileUri Uri { get; }
        IVolumeSetItem Volume { get; }
    }

    public interface IFileUriKey<TVolume> : IFileUriKey
        where TVolume : IVolumeSetItem
    {
        new TVolume Volume { get; }
    }
}
