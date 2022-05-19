using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for an <see cref="IUpstreamTagDefinition"/> that is associated with an <see cref="IUpstreamFile"/>.
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="M.IFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamFile, IUpstreamTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamFileTag}" />
    /// <seealso cref="Local.Model.IFileTag" />
    public interface IUpstreamFileTag : IUpstreamItemTag, M.IFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamTagDefinition>, IEquatable<IUpstreamFileTag>
    {
        /// <summary>
        /// Gets the tagged file.
        /// </summary>
        /// <value>The tagged <see cref="IUpstreamFile"/>.</value>
        new IUpstreamFile Tagged { get; }
    }
}
