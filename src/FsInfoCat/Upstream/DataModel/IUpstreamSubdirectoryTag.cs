using System;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSubdirectoryTag : IUpstreamItemTag, ISubdirectoryTag, IHasMembershipKeyReference<IUpstreamSubdirectory, IUpstreamTagDefinition>, IEquatable<IUpstreamSubdirectoryTag>
    {
        new IUpstreamSubdirectory Tagged { get; }
    }
}
