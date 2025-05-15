using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="IDbContext.PersonalSubdirectoryTags"/>
    public interface IPersonalSubdirectoryTag : IPersonalTag, ISubdirectoryTag, IEquatable<IPersonalSubdirectoryTag>,
        IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition> { }
}
