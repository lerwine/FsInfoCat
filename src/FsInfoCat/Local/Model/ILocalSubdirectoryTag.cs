using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface ILocalSubdirectoryTag
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSubdirectoryTag}" />
    /// <seealso cref="Upstream.Model.IUpstreamSubdirectoryTag" />
    public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>, IEquatable<ILocalSubdirectoryTag>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ILocalSubdirectory"/>.</value>
        new ILocalSubdirectory Tagged { get; }
    }
}
