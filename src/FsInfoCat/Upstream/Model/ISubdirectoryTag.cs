using FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Interface IUpstreamSubdirectoryTag
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSubdirectoryTag}" />
    /// <seealso cref="Local.Model.ISubdirectoryTag" />
    public interface IUpstreamSubdirectoryTag : IUpstreamItemTag, ISubdirectoryTag, IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamTagDefinition>,
        IEquatable<IUpstreamSubdirectoryTag>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="IUpstreamSubdirectory"/>.</value>
        new IUpstreamSubdirectory Tagged { get; }
    }
}
