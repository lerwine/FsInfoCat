using System;
using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedTagDefinition : IUpstreamTagDefinition, ISharedTagDefinition, IEquatable<IUpstreamSharedTagDefinition>
    {
        new IEnumerable<IUpstreamSharedFileTag> FileTags { get; }

        new IEnumerable<IUpstreamSharedSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamSharedVolumeTag> VolumeTags { get; }
    }
}
