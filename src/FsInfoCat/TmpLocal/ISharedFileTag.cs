using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="M.ISharedFileTag" />
    /// <seealso cref="ILocalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalFile, ILocalSharedTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSharedFileTag}" />
    /// <seealso cref="Upstream.Model.ISharedFileTag" />
    public interface ILocalSharedFileTag : ILocalSharedTag, M.ISharedFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition>,
        IEquatable<ILocalSharedFileTag> { }
}
