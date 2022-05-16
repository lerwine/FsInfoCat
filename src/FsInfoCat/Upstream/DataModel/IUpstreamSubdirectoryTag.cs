using System;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Interface IUpstreamSubdirectoryTag
    /// Implements the <see cref="IUpstreamItemTag" />
    /// Implements the <see cref="ISubdirectoryTag" />
    /// Implements the <see cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamTagDefinition}" />
    /// </summary>
    /// <seealso cref="IUpstreamItemTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamSubdirectory, IUpstreamTagDefinition}" />
    /// <seealso cref="IEquatable{IUpstreamSubdirectoryTag}" />
    /// <seealso cref="Local.ILocalSubdirectoryTag" />
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
