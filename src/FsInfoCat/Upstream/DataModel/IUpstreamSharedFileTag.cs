using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedFileTag : IUpstreamSharedTag, ISharedFileTag, IUpstreamFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamSharedTagDefinition>, IEquatable<IUpstreamSharedFileTag> { }
}
