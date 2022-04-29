using System;

namespace FsInfoCat
{
    public interface ISharedFileTag : ISharedTag, IFileTag, IEquatable<ISharedFileTag>, IHasMembershipKeyReference<IFile, ISharedTagDefinition> { }
}
