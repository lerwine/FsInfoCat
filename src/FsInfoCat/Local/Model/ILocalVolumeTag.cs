using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalVolume, ILocalTagDefinition}" />
    /// <seealso cref="Upstream.Model.IUpstreamVolumeTag" />
    /// <seealso cref="IEquatable{ILocalVolumeTag}" />
    public interface ILocalVolumeTag : ILocalItemTag, IVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalTagDefinition>, IEquatable<ILocalVolumeTag>
    {
        /// <summary>
        /// Gets the tagged volume.
        /// </summary>
        /// <value>The tagged <see cref="ILocalVolume"/>.</value>
        new ILocalVolume Tagged { get; }
    }
}
