using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="ILocalVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalVolume, ILocalSharedTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSharedVolumeTag}" />
    /// <seealso cref="Upstream.Model.IUpstreamSharedVolumeTag" />
    public interface ILocalSharedVolumeTag : ILocalSharedTag, ISharedVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition>,
        IEquatable<ILocalSharedVolumeTag> { }
}
