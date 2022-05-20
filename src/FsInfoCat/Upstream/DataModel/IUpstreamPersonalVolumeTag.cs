using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="IUpstreamVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamVolume, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalVolumeTag}" />
    /// <seealso cref="Local.ILocalPersonalVolumeTag" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPersonalVolumeTag")]
    public interface IUpstreamPersonalVolumeTag : IUpstreamPersonalTag, IPersonalVolumeTag, IUpstreamVolumeTag,
        IHasMembershipKeyReference<IUpstreamVolume, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalVolumeTag> { }
}
