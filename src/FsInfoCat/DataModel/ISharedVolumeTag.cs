using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="IEquatable{ISharedVolumeTag}" />
    /// <seealso cref="IHasMembershipKeyReference{IVolume, ISharedTagDefinition}" />
    /// <seealso cref="Local.ILocalSharedVolumeTag" />
    /// <seealso cref="Upstream.IUpstreamSharedVolumeTag" />
    public interface ISharedVolumeTag : ISharedTag, IVolumeTag, IEquatable<ISharedVolumeTag>, IHasMembershipKeyReference<IVolume, ISharedTagDefinition> { }
}
