using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalSubdirectory"/>.
    /// </summary>
    /// <seealso cref="ILocalPersonalTag" />
    /// <seealso cref="M.IPersonalSubdirectoryTag" />
    /// <seealso cref="ILocalSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalPersonalSubdirectoryTag}" />
    /// <seealso cref="Upstream.Model.IPersonalSubdirectoryTag" />
    public interface ILocalPersonalSubdirectoryTag : ILocalPersonalTag, M.IPersonalSubdirectoryTag, ILocalSubdirectoryTag,
        IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition>, IEquatable<ILocalPersonalSubdirectoryTag> { }
}
