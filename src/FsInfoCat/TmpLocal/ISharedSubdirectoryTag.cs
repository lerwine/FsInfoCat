using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalSubdirectory"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="M.ISharedSubdirectoryTag" />
    /// <seealso cref="ILocalSubdirectoryTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalSubdirectory, ILocalSharedTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSharedSubdirectoryTag}" />
    /// <seealso cref="Upstream.Model.ISharedSubdirectoryTag" />
    public interface ILocalSharedSubdirectoryTag : ILocalSharedTag, M.ISharedSubdirectoryTag, ILocalSubdirectoryTag,
        IHasMembershipKeyReference<ILocalSubdirectory, ILocalSharedTagDefinition>, IEquatable<ILocalSharedSubdirectoryTag> { }
}
