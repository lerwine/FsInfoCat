using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="IVolumeTag" />
    /// <seealso cref="IEquatable{IPersonalVolumeTag}" />
    /// <seealso cref="IHasMembershipKeyReference{IVolume, IPersonalTagDefinition}" />
    public interface IPersonalVolumeTag : IPersonalTag, IVolumeTag, IEquatable<IPersonalVolumeTag>, IHasMembershipKeyReference<IVolume, IPersonalTagDefinition> { }
}
