using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IPersonalTagDefinition : ITagDefinition
    {
        new IEnumerable<IPersonalFileTag> FileTags { get; }

        new IEnumerable<IPersonalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IPersonalVolumeTag> VolumeTags { get; }
    }
}
