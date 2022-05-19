using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Interface ILocalSubdirectoryTag
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="M.ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSubdirectoryTag}" />
    /// <seealso cref="Upstream.Model.ISubdirectoryTag" />
    public interface ILocalSubdirectoryTag : ILocalItemTag, M.ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>, IEquatable<ILocalSubdirectoryTag>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ILocalSubdirectory"/>.</value>
        new ILocalSubdirectory Tagged { get; }
    }
}
