using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalFileTag : IUpstreamPersonalTag, IPersonalFileTag, IUpstreamFileTag, IHasMembershipKeyReference<IUpstreamFile, IUpstreamPersonalTagDefinition>, IEquatable<IUpstreamPersonalFileTag> { }
}
