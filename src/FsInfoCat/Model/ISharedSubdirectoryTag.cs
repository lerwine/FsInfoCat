using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IEquatable{ISharedSubdirectoryTag}" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, ISharedTagDefinition}" />
    /// <seealso cref="Local.Model.ILocalSharedSubdirectoryTag" />
    /// <seealso cref="Upstream.Model.IUpstreamSharedSubdirectoryTag" />
    /// <seealso cref="ISubdirectory.SharedTags" />
    /// <seealso cref="ISharedTagDefinition.SubdirectoryTags" />
    /// <seealso cref="IDbContext.SharedSubdirectoryTags" />
    public interface ISharedSubdirectoryTag : ISharedTag, ISubdirectoryTag, IEquatable<ISharedSubdirectoryTag>, IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition> { }
}
