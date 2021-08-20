using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalTagDefinition : ILocalDbEntity, ITagDefinition
    {
        new IEnumerable<ILocalFileTag> FileTags { get; }

        new IEnumerable<ILocalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ILocalVolumeTag> VolumeTags { get; }
    }
}
