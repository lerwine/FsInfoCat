using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
    /// </summary>
    /// <seealso cref="ILocalPersonalTag" />
    /// <seealso cref="IPersonalFileTag" />
    /// <seealso cref="ILocalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalFile, ILocalPersonalTagDefinition}" />
    public interface ILocalPersonalFileTag : ILocalPersonalTag, IPersonalFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition>, IEquatable<ILocalPersonalFileTag> { }
}
