using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamPersonalTagDefinition : IUpstreamTagDefinition, IPersonalTagDefinition
    {
        IUserProfile User { get; }

        new IEnumerable<IUpstreamPersonalFileTag> FileTags { get; }

        new IEnumerable<IUpstreamPersonalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamPersonalVolumeTag> VolumeTags { get; }
    }
}
