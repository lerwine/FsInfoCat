using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamTagDefinition"/> that is associated with an <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamVolume, IUpstreamTagDefinition}" />
    /// <seealso cref="Local.ILocalVolumeTag" />
    public interface IUpstreamVolumeTag : IUpstreamItemTag, IVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamTagDefinition>, IEquatable<IUpstreamVolumeTag>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="IUpstreamVolume"/>.</value>
        new IUpstreamVolume Tagged { get; }
    }
}
