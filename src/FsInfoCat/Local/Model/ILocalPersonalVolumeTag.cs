using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalVolume"/>.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamPersonalVolumeTag" />
public interface ILocalPersonalVolumeTag : ILocalPersonalTag, IPersonalVolumeTag, ILocalVolumeTag, IHasMembershipKeyReference<ILocalVolume, ILocalPersonalTagDefinition>,
    IEquatable<ILocalPersonalVolumeTag> { }
