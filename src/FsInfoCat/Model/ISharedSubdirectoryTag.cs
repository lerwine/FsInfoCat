using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IDbContext.SharedSubdirectoryTags"/>
    public interface ISharedSubdirectoryTag : ISharedTag, ISubdirectoryTag, IEquatable<ISharedSubdirectoryTag>, IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition> { }
}
