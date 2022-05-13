using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolumeTag : IUpstreamItemTag, IVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamTagDefinition>, IEquatable<IUpstreamVolumeTag>
    {
        new IUpstreamVolume Tagged { get; }
    }
}
