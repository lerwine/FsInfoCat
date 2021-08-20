using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalPersonalTagDefinition : ILocalTagDefinition, IPersonalTagDefinition
    {
        new IEnumerable<ILocalPersonalFileTag> FileTags { get; }

        new IEnumerable<ILocalPersonalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ILocalPersonalVolumeTag> VolumeTags { get; }
    }
}
