using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedSubdirectoryTag : IUpstreamSharedTag, ISharedSubdirectoryTag, IUpstreamSubdirectoryTag, IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamSharedTagDefinition>, IEquatable<IUpstreamSharedSubdirectoryTag> { }
}
