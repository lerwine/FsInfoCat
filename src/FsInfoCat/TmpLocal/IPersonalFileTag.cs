using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
    /// </summary>
    /// <seealso cref="ILocalPersonalTag" />
    /// <seealso cref="M.IPersonalFileTag" />
    /// <seealso cref="ILocalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalFile, ILocalPersonalTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalPersonalFileTag}" />
    /// <seealso cref="Upstream.Model.IPersonalFileTag" />
    public interface ILocalPersonalFileTag : ILocalPersonalTag, M.IPersonalFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>,
        IEquatable<ILocalPersonalFileTag> { }
}
