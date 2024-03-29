using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IEquatable{IPersonalFileTag}" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, IPersonalTagDefinition}" />
    /// <seealso cref="Local.Model.ILocalPersonalFileTag" />
    /// <seealso cref="Upstream.Model.IUpstreamPersonalFileTag" />
    /// <seealso cref="IFile.PersonalTags" />
    /// <seealso cref="IDbContext.PersonalFileTags" />
    public interface IPersonalFileTag : IPersonalTag, IFileTag, IEquatable<IPersonalFileTag>, IHasMembershipKeyReference<IFile, IPersonalTagDefinition> { }
}
