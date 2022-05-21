using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="IEquatable{IPersonalVolumeTag}" />
    /// <seealso cref="IHasMembershipKeyReference{IVolume, IPersonalTagDefinition}" />
    /// <seealso cref="Local.Model.ILocalPersonalVolumeTag" />
    /// <seealso cref="Upstream.Model.IUpstreamPersonalVolumeTag" />
    /// <seealso cref="IVolume.PersonalTags" />
    /// <seealso cref="IDbContext.PersonalVolumeTags" />
    public interface IPersonalVolumeTag : IPersonalTag, IVolumeTag, IEquatable<IPersonalVolumeTag>, IHasMembershipKeyReference<IVolume, IPersonalTagDefinition> { }
}
