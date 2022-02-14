using System;

namespace FsInfoCat
{
    public interface ISharedVolumeTag : ISharedTag, IVolumeTag, IEquatable<ISharedVolumeTag> { }
}
