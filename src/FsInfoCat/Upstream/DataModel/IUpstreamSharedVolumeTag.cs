using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamSharedTagDefinition"/> that is associated with an <see cref="IUpstreamVolume"/>.
    /// </summary>
    /// <seealso cref="IUpstreamSharedTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IUpstreamVolumeTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamVolume, IUpstreamSharedTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSharedVolumeTag}" />
    /// <seealso cref="Local.ILocalSharedVolumeTag" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamSharedVolumeTag")]
    public interface IUpstreamSharedVolumeTag : IUpstreamSharedTag, ISharedVolumeTag, IUpstreamVolumeTag, IHasMembershipKeyReference<IUpstreamVolume, IUpstreamSharedTagDefinition>,
        IEquatable<IUpstreamSharedVolumeTag> { }
}
