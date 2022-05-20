using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamPersonalTagDefinition"/> that is associated with an <see cref="IUpstreamSubdirectory"/>.
    /// </summary>
    /// <seealso cref="IUpstreamPersonalTag" />
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="IUpstreamSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamPersonalSubdirectoryTag}" />
    /// <seealso cref="Local.ILocalPersonalSubdirectoryTag" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPersonalSubdirectoryTag")]
    public interface IUpstreamPersonalSubdirectoryTag : IUpstreamPersonalTag, IPersonalSubdirectoryTag, IUpstreamSubdirectoryTag,
        IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalSubdirectoryTag> { }
}
