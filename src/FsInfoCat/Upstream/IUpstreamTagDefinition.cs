using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamTagDefinition : IUpstreamDbEntity, ITagDefinition
    {
        new IEnumerable<IUpstreamFileTag> FileTags { get; }

        new IEnumerable<IUpstreamSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamVolumeTag> VolumeTags { get; }
    }
}
