using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="ISharedTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="ISharedTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IEquatable{ISharedSubdirectoryTag}" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, ISharedTagDefinition}" />
    public interface ISharedSubdirectoryTag : ISharedTag, ISubdirectoryTag, IEquatable<ISharedSubdirectoryTag>, IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition> { }
}
