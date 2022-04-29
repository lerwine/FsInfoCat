using System;

namespace FsInfoCat
{
    public interface IPersonalFileTag : IPersonalTag, IFileTag, IEquatable<IPersonalFileTag>, IHasMembershipKeyReference<IFile, IPersonalTagDefinition> { }
}
