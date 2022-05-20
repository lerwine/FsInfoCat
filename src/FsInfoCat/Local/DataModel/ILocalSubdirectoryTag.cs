using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface ILocalSubdirectoryTag
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSubdirectoryTag}" />
    /// <seealso cref="Upstream.IUpstreamSubdirectoryTag" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSubdirectoryTag")]
    public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>, IEquatable<ILocalSubdirectoryTag>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ILocalSubdirectory"/>.</value>
        new ILocalSubdirectory Tagged { get; }
    }
}
