using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalVolumeTag : IUpstreamPersonalTag, IPersonalVolumeTag, IUpstreamVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalVolumeTag> { }
}
