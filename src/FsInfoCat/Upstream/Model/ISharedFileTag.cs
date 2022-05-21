using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamSharedTagDefinition"/> that is associated with an <see cref="IUpstreamFile"/>.
    /// </summary>
    /// <seealso cref="IUpstreamSharedTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="IUpstreamFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamFile, IUpstreamSharedTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSharedFileTag}" />
    /// <seealso cref="Local.Model.ISharedFileTag" />
    public interface IUpstreamSharedFileTag : IUpstreamSharedTag, ISharedFileTag, IUpstreamFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamSharedTagDefinition>,
        IEquatable<IUpstreamSharedFileTag> { }
}
