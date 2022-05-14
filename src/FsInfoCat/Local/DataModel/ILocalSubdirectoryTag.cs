using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface ILocalSubdirectoryTag
    /// Implements the <see cref="ILocalItemTag" />
    /// Implements the <see cref="ISubdirectoryTag" />
    /// Implements the <see cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalTagDefinition}" />
    /// </summary>
    /// <seealso cref="ILocalItemTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalTagDefinition}" />
    public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>, IEquatable<ILocalSubdirectoryTag>
    {
        /// <summary>
        /// Gets the tagged subdirectory.
        /// </summary>
        /// <value>The tagged <see cref="ILocalSubdirectory"/>.</value>
        new ILocalSubdirectory Tagged { get; }
    }
}
