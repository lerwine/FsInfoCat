using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an <see cref="IPersonalTagDefinition"/> that is associated with an <see cref="ISubdirectory"/>.
    /// </summary>
    /// <seealso cref="IPersonalTag" />
    /// <seealso cref="ISubdirectoryTag" />
    /// <seealso cref="IEquatable{IPersonalSubdirectoryTag}" />
    /// <seealso cref="IHasMembershipKeyReference{ISubdirectory, IPersonalTagDefinition}" />
    public interface IPersonalSubdirectoryTag : IPersonalTag, ISubdirectoryTag, IEquatable<IPersonalSubdirectoryTag>,
        IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition> { }
}
