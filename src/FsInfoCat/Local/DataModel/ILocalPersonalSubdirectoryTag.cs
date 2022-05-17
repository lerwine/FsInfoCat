using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalSubdirectory"/>.
    /// </summary>
    /// <seealso cref="ILocalPersonalTag" />
    /// <seealso cref="IPersonalSubdirectoryTag" />
    /// <seealso cref="ILocalSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalPersonalSubdirectoryTag}" />
    /// <seealso cref="Upstream.IUpstreamPersonalSubdirectoryTag" />
    public interface ILocalPersonalSubdirectoryTag : ILocalPersonalTag, IPersonalSubdirectoryTag, ILocalSubdirectoryTag,
        IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition>, IEquatable<ILocalPersonalSubdirectoryTag> { }
}
