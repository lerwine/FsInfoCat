using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamTagDefinition"/> that is associated with an <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="M.IVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamVolume, IUpstreamTagDefinition}" />
    /// <seealso cref="Local.Model.IVolumeTag" />
    public interface IUpstreamVolumeTag : IUpstreamItemTag, M.IVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamTagDefinition>, IEquatable<IUpstreamVolumeTag>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="IUpstreamVolume"/>.</value>
        new IUpstreamVolume Tagged { get; }
    }
}
