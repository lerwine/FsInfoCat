using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="IDbContext.SharedVolumeTags"/>
    public interface ISharedVolumeTag : ISharedTag, IVolumeTag, IEquatable<ISharedVolumeTag>, IHasMembershipKeyReference<IVolume, ISharedTagDefinition> { }
}
