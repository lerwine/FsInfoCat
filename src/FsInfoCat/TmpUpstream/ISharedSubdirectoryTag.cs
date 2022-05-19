using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamSharedTagDefinition"/> that is associated with an <see cref="IUpstreamSubdirectory"/>.
    /// </summary>
    /// <seealso cref="IUpstreamSharedTag" />
    /// <seealso cref="M.ISharedSubdirectoryTag" />
    /// <seealso cref="IUpstreamSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamSharedTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSharedSubdirectoryTag}" />
    /// <seealso cref="Local.Model.ISharedSubdirectoryTag" />
    public interface IUpstreamSharedSubdirectoryTag : IUpstreamSharedTag, M.ISharedSubdirectoryTag, IUpstreamSubdirectoryTag,
        IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamSharedTagDefinition>, IEquatable<IUpstreamSharedSubdirectoryTag> { }
}
