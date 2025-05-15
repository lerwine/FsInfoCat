using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="IDbContext.PersonalVolumeTags"/>
    public interface IPersonalVolumeTag : IPersonalTag, IVolumeTag, IEquatable<IPersonalVolumeTag>, IHasMembershipKeyReference<IVolume, IPersonalTagDefinition> { }
}
