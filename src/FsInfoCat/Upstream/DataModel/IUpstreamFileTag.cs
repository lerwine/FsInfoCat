using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamTagDefinition"/> that is associated with an <see cref="IUpstreamFile"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="IFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamFile, IUpstreamTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamFileTag}" />
    /// <seealso cref="Local.ILocalFileTag" />
    public interface IUpstreamFileTag : IUpstreamItemTag, IFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamTagDefinition>, IEquatable<IUpstreamFileTag>
    {
        /// <summary>
        /// Gets the tagged file.
        /// </summary>
        /// <value>The tagged <see cref="IUpstreamFile"/>.</value>
        new IUpstreamFile Tagged { get; }
    }
}
