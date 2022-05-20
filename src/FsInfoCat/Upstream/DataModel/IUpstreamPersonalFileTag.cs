using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamFile"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamFile, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalFileTag}" />
    /// <seealso cref="Local.ILocalPersonalFileTag" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPersonalFileTag")]
    public interface IUpstreamPersonalFileTag : IUpstreamPersonalTag, IPersonalFileTag, IUpstreamFileTag,
        IHasMembershipKeyReference<IUpstreamFile, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalFileTag> { }
}
