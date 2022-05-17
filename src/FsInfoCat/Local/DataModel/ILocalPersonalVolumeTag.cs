using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalPersonalTag" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="ILocalVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalVolume, ILocalPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalPersonalVolumeTag}" />
    /// <seealso cref="Upstream.IUpstreamPersonalVolumeTag" />
    public interface ILocalPersonalVolumeTag : ILocalPersonalTag, IPersonalVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalPersonalTagDefinition>,
        IEquatable<ILocalPersonalVolumeTag> { }
}
