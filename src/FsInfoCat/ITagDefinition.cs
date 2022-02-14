using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ITagDefinition : ITagDefinitionRow
    {
        IEnumerable<IFileTag> FileTags { get; }

        IEnumerable<ISubdirectoryTag> SubdirectoryTags { get; }

        IEnumerable<IVolumeTag> VolumeTags { get; }
    }
}
