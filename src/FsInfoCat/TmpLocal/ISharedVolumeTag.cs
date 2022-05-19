using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="M.ISharedVolumeTag" />
    /// <seealso cref="ILocalVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalVolume, ILocalSharedTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSharedVolumeTag}" />
    /// <seealso cref="Upstream.Model.ISharedVolumeTag" />
    public interface ILocalSharedVolumeTag : ILocalSharedTag, M.ISharedVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalSharedTagDefinition>,
        IEquatable<ILocalSharedVolumeTag> { }
}
