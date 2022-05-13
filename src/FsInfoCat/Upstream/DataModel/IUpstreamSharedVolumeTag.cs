using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedVolumeTag : IUpstreamSharedTag, ISharedVolumeTag, IUpstreamVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamSharedTagDefinition>, IEquatable<IUpstreamSharedVolumeTag> { }
}
