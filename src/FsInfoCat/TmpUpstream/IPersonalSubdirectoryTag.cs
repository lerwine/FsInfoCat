using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamSubdirectory"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="M.IPersonalSubdirectoryTag" />
    /// <seealso cref="IUpstreamSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalSubdirectoryTag}" />
    /// <seealso cref="Local.Model.IPersonalSubdirectoryTag" />
    public interface IUpstreamPersonalSubdirectoryTag : IUpstreamPersonalTag, M.IPersonalSubdirectoryTag, IUpstreamSubdirectoryTag,
        IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalSubdirectoryTag> { }
}
