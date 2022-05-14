using System;
using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamTagDefinition : IUpstreamTagDefinitionRow, ITagDefinition, IEquatable<IUpstreamTagDefinition>
    {
        new IEnumerable<IUpstreamFileTag> FileTags { get; }

        new IEnumerable<IUpstreamSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamVolumeTag> VolumeTags { get; }
    }
}
