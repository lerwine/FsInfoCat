using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamSharedTagDefinition"/> that is associated with an <see cref="IUpstreamSubdirectory"/>.
    /// </summary>
    /// <seealso cref="IUpstreamSharedTag" />
    /// <seealso cref="ISharedSubdirectoryTag" />
    /// <seealso cref="IUpstreamSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamSharedTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSharedSubdirectoryTag}" />
    /// <seealso cref="Local.Model.ILocalSharedSubdirectoryTag" />
    public interface IUpstreamSharedSubdirectoryTag : IUpstreamSharedTag, ISharedSubdirectoryTag, IUpstreamSubdirectoryTag,
        IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamSharedTagDefinition>, IEquatable<IUpstreamSharedSubdirectoryTag> { }
}
