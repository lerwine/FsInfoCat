using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSharedTagDefinition : IUpstreamTagDefinition, ISharedTagDefinition
    {
        new IEnumerable<IUpstreamSharedFileTag> FileTags { get; }

        new IEnumerable<IUpstreamSharedSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamSharedVolumeTag> VolumeTags { get; }
    }
}
