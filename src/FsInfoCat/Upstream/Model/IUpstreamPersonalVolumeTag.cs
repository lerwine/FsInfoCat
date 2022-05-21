using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="IUpstreamVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamVolume, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalVolumeTag}" />
    /// <seealso cref="Local.Model.ILocalPersonalVolumeTag" />
    public interface IUpstreamPersonalVolumeTag : IUpstreamPersonalTag, IPersonalVolumeTag, IUpstreamVolumeTag,
        IHasMembershipKeyReference<IUpstreamVolume, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalVolumeTag> { }
}
