using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamFile"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="M.IPersonalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamFile, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalFileTag}" />
    /// <seealso cref="Local.Model.IPersonalFileTag" />
    public interface IUpstreamPersonalFileTag : IUpstreamPersonalTag, M.IPersonalFileTag, IUpstreamFileTag,
        IHasMembershipKeyReference<IUpstreamFile, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalFileTag> { }
}
