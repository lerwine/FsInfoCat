using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="ISharedVolumeTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IDbContext.SharedFileTags"/>
    public interface ISharedFileTag : ISharedTag, IFileTag, IEquatable<ISharedFileTag>, IHasMembershipKeyReference<IFile, ISharedTagDefinition> { }
}
