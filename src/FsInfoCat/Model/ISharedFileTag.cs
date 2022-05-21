using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IEquatable{ISharedFileTag}" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, ISharedTagDefinition}" />
    /// <seealso cref="Local.ILocalSharedFileTag" />
    /// <seealso cref="Upstream.IUpstreamSharedFileTag" />
    /// <seealso cref="IFile.SharedTags" />
    /// <seealso cref="ISharedTagDefinition.FileTags" />
    /// <seealso cref="IDbContext.SharedFileTags" />
    public interface ISharedFileTag : ISharedTag, IFileTag, IEquatable<ISharedFileTag>, IHasMembershipKeyReference<IFile, ISharedTagDefinition> { }
}
