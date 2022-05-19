using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IEquatable{IPersonalSubdirectoryTag}" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, IPersonalTagDefinition}" />
    /// <seealso cref="Local.ILocalPersonalSubdirectoryTag" />
    /// <seealso cref="Upstream.IUpstreamPersonalSubdirectoryTag" />
    /// <seealso cref="ISubdirectory.PersonalTags" />
    /// <seealso cref="IDbContext.PersonalSubdirectoryTags" />
    public interface IPersonalSubdirectoryTag : IPersonalTag, ISubdirectoryTag, IEquatable<IPersonalSubdirectoryTag>,
        IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition> { }
}
