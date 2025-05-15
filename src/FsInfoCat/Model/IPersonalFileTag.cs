using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="IPersonalVolumeTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="IDbContext.PersonalFileTags"/>
    public interface IPersonalFileTag : IPersonalTag, IFileTag, IEquatable<IPersonalFileTag>, IHasMembershipKeyReference<IFile, IPersonalTagDefinition> { }
}
