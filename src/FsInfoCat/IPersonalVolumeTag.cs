using System;

namespace FsInfoCat
{
    public interface IPersonalVolumeTag : IPersonalTag, IVolumeTag, IEquatable<IPersonalVolumeTag>, IHasMembershipKeyReference<IVolume, IPersonalTagDefinition> { }
}
